namespace ServiceConstruct.Tests;

using GamesWithGravitas.ServiceConstruct;
using Microsoft;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using System.Reflection;
using System.Text;
using VerifyCS = CSharpSourceGeneratorVerifier<GamesWithGravitas.ServiceConstruct.Generator>;

public class EmbeddedResourceTests
{
    private static readonly Assembly TestAssembly = typeof(EmbeddedResourceTests).Assembly;

    private static readonly Assembly ServiceConstructAssembly = typeof(ServiceConstructAttribute).Assembly;

    private static readonly MetadataReference ServiceConstructAssemblyMetatdataReference = MetadataReference.CreateFromFile(ServiceConstructAssembly.Location);

    [Theory]
    [InlineData("NoNamespace")]
    [InlineData("SimpleNamespace")]
    [InlineData("FileScopedNamespace")]
    [InlineData("DifferentNamespaces")]
    [InlineData("MultipleServices")]
    [InlineData("CustomMethodName")]
    public async Task Test(string name)
    {
        (var sources, var generatedSources) = await LoadAsync(name);

        Assert.True(sources.Any());
        Assert.True(generatedSources.Any());

        var test = new VerifyCS.Test();
        test.TestState.AdditionalReferences.Add(ServiceConstructAssemblyMetatdataReference);
        test.TestState.Sources.AddRange(sources);
        test.TestState.GeneratedSources.AddRange(generatedSources);
        await test.RunAsync();
    }

    private async Task<(SourceFileList Sources, SourceFileCollection GeneratedSources)> LoadAsync(string name)
    {
        var sourcesPrefix = $"{TestAssembly.GetName().Name}.TestCases.{name}.Sources.";
        var generatedSourcesPrefix = $"{TestAssembly.GetName().Name}.TestCases.{name}.GeneratedSources.";
        var expectedGeneratedSourcePrefix = $"{ServiceConstructAssembly.GetName().Name}/GamesWithGravitas.ServiceConstruct.Generator/";
 
        var resources = TestAssembly.GetManifestResourceNames();

        var sources = new SourceFileList("", "");
        var sourceResourceNames = resources.Where(x => x.StartsWith(sourcesPrefix));
        foreach (var resourceName in sourceResourceNames)
        {
            using var stream = TestAssembly.GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(stream!);
            var content = await streamReader.ReadToEndAsync();
            var source = CSharpSyntaxTree.ParseText(SourceText.From(content, Encoding.UTF8)).GetRoot().NormalizeWhitespace().SyntaxTree.GetText();
            sources.Add((resourceName.Replace(sourcesPrefix, ""), source));
        }

        var generatedSources = new SourceFileCollection();
        var generatedSourceResourceNames = resources.Where(x => x.StartsWith(generatedSourcesPrefix));
        foreach (var resourceName in generatedSourceResourceNames)
        {
            using var stream = TestAssembly.GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(stream!);
            var content = await streamReader.ReadToEndAsync();
            var source = CSharpSyntaxTree.ParseText(SourceText.From(content, Encoding.UTF8)).GetRoot().NormalizeWhitespace().SyntaxTree.GetText();
            generatedSources.Add((resourceName.Replace(generatedSourcesPrefix, expectedGeneratedSourcePrefix), source));
        }

        return (sources, generatedSources);
    }
}

