using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ServiceConstruct
{

    [Generator]
    public class Generator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var finder = (ServiceConstructFinder)context.SyntaxReceiver;
            foreach ((var classDeclaration, var parameterList) in finder.ServiceConstructItems)
            {
                var namespaceName = GetNamespace(classDeclaration);
                var className = classDeclaration.Identifier.ToString();

                var sourceText = new StringBuilder();
                if (namespaceName != null)
                {
                    sourceText.Append($"namespace {namespaceName} {{");
                }
                sourceText.Append($"public partial class {className} {{");
                sourceText.Append($"public static {className} ServiceConstruct(System.IServiceProvider serviceProvider) {{");
                sourceText.Append($"return new {className}(");
                bool hasAddedParameter = false;
                foreach (var parameter in parameterList.Parameters)
                {
                    if (hasAddedParameter)
                    {
                        sourceText.Append(", ");
                    }
                    var typeName = parameter.Type.ToString();
                    sourceText.Append($"({typeName})serviceProvider.GetService(typeof({typeName}))");
                    hasAddedParameter = true;
                }
                sourceText.Append($");");
                sourceText.Append("}");
                sourceText.Append("}");
                if (namespaceName != null)
                {
                    sourceText.Append("}");
                }

                    var source = CSharpSyntaxTree.ParseText(SourceText.From(sourceText.ToString(), Encoding.UTF8)).GetRoot().NormalizeWhitespace().SyntaxTree.GetText();

                context.AddSource($"{className}.g.cs", source);
            }
        }

        private string? GetNamespace(ClassDeclarationSyntax classDeclaration)
        {
            var namespaceDeclaration = classDeclaration.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
            if (namespaceDeclaration != null)
            {
                return namespaceDeclaration.Name.ToString();
            }
            return null;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ServiceConstructFinder());
        }
    }

    public class ServiceConstructFinder : ISyntaxReceiver
    {
        public List<(ClassDeclarationSyntax, ParameterListSyntax)> ServiceConstructItems { get; } = new List<(ClassDeclarationSyntax, ParameterListSyntax)>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ConstructorDeclarationSyntax constructor)
            {
                var attributes = constructor.AttributeLists.SelectMany(x => x.Attributes);
                foreach (var attribute in attributes.Where(x => x.Name.ToString().EndsWith("ServiceConstruct", StringComparison.Ordinal)))
                {
                    var classDeclaration = constructor.Parent as ClassDeclarationSyntax;
                    ServiceConstructItems.Add((classDeclaration, constructor.ParameterList));
                }
            }
        }
    }
}

