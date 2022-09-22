using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;

namespace ServiceConstruct.Tests;

public abstract class BaseTest
{
    protected async Task<(SourceFileList Sources, SourceFileCollection GeneratedSources)> LoadAsync(string name)
    {
        var sourcesPrefix = $"ServiceConstruct.Tests.{name}.Sources.";
        var generatedSourcesPrefix = $"ServiceConstruct.Tests.{name}.GeneratedSources.";
        var expectedGeneratedSourcePrefix = "ServiceConstruct/ServiceConstruct.Generator/";
        var assembly = typeof(BaseTest).Assembly;
        var resources = assembly.GetManifestResourceNames();

        var sources = new SourceFileList("", "");
        var sourceResourceNames = resources.Where(x => x.StartsWith(sourcesPrefix));
        foreach (var resourceName in sourceResourceNames)
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(stream!);
            var content = await streamReader.ReadToEndAsync();
            var source = CSharpSyntaxTree.ParseText(SourceText.From(content, Encoding.UTF8)).GetRoot().NormalizeWhitespace().SyntaxTree.GetText();
            sources.Add((resourceName.Replace(sourcesPrefix, ""), source));
        }

        var generatedSources = new SourceFileCollection();
        var generatedSourceResourceNames = resources.Where(x => x.StartsWith(generatedSourcesPrefix));
        foreach (var resourceName in generatedSourceResourceNames)
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(stream!);
            var content = await streamReader.ReadToEndAsync();
            var source = CSharpSyntaxTree.ParseText(SourceText.From(content, Encoding.UTF8)).GetRoot().NormalizeWhitespace().SyntaxTree.GetText();
            generatedSources.Add((resourceName.Replace(generatedSourcesPrefix, expectedGeneratedSourcePrefix), source));
        }

        return (sources, generatedSources);
    }
}

