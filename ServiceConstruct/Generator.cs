using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GamesWithGravitas.ServiceConstruct
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var finder = (ServiceConstructFinder)context.SyntaxReceiver!;
            foreach ((var classDeclaration, var constructorDeclaration) in finder.ServiceConstructItems)
            {
                var parameterList = constructorDeclaration.ParameterList;
                var semanticModel = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
                var constructorSymbol = semanticModel.GetDeclaredSymbol(constructorDeclaration);

                if (classSymbol is null)
                {
                    return;
                }

                var namespaceName = classSymbol.ContainingNamespace.Name;
                var className = classDeclaration.Identifier.ToString();
                var attribute = constructorSymbol?.GetAttributes().FirstOrDefault(x => x.AttributeClass?.Name.Equals("ServiceConstructAttribute", StringComparison.Ordinal) ?? false);
                var methodName = attribute?.NamedArguments.FirstOrDefault(x => x.Key.Equals("MethodName", StringComparison.OrdinalIgnoreCase)).Value.Value;
                methodName = string.IsNullOrEmpty(methodName as string) ? "ServiceConstruct" : methodName;
                var sourceText = new StringBuilder();
                if (!string.IsNullOrEmpty(namespaceName))
                {
                    sourceText.Append($"namespace {namespaceName} {{");
                }
                sourceText.Append($"public partial class {className} {{");
                sourceText.Append($"public static {className} {methodName}(global::System.IServiceProvider serviceProvider) {{");
                sourceText.Append($"return new {className}(");
                bool hasAddedParameter = false;
                foreach (var parameter in parameterList.Parameters)
                {
                    if (hasAddedParameter)
                    {
                        sourceText.Append(", ");
                    }
                    var parameterSymbol = semanticModel.GetDeclaredSymbol(parameter);
                    var typeName = parameterSymbol.Type.ToMinimalDisplayString(semanticModel, 0);
                    sourceText.Append($"({typeName})serviceProvider.GetService(typeof({typeName}))");
                    hasAddedParameter = true;
                }
                sourceText.Append($");");
                sourceText.Append("}");
                sourceText.Append("}");
                if (!string.IsNullOrEmpty(namespaceName))
                {
                    sourceText.Append("}");
                }

                var source = CSharpSyntaxTree.ParseText(SourceText.From(sourceText.ToString(), Encoding.UTF8)).GetRoot().NormalizeWhitespace().SyntaxTree.GetText();

                context.AddSource($"{className}.g.cs", source);
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ServiceConstructFinder());
        }
    }

    public class ServiceConstructFinder : ISyntaxReceiver
    {
        public List<(ClassDeclarationSyntax, ConstructorDeclarationSyntax)> ServiceConstructItems { get; } = new List<(ClassDeclarationSyntax, ConstructorDeclarationSyntax)>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ConstructorDeclarationSyntax constructor)
            {
                var attributes = constructor.AttributeLists.SelectMany(x => x.Attributes);
                foreach (var attribute in attributes.Where(x => x.Name.ToString().EndsWith("ServiceConstruct", StringComparison.Ordinal)))
                {
                    var classDeclaration = constructor.Parent as ClassDeclarationSyntax;
                    ServiceConstructItems.Add((classDeclaration, constructor));
                }
            }
        }
    }
}

