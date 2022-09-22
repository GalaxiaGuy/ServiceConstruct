using Microsoft.CodeAnalysis.Testing;

namespace ServiceConstruct.Tests;

public abstract class BaseTest
{
    protected async Task<(SourceFileList Sources, SourceFileCollection GeneratedSources)> LoadAsync(string name)
    {
        var sourcesPrefix = $"ServiceConstruct.Tests.{name}.Sources.";
        var generatedSourcesPrefix = $"ServiceConstruct.Tests.{name}.GeneratedSources.";
        var assembly = typeof(BaseTest).Assembly;
        var resources = assembly.GetManifestResourceNames();

        var sources = new SourceFileList("", "");
        var sourceResourceNames = resources.Where(x => x.StartsWith(sourcesPrefix));
        foreach (var resourceName in sourceResourceNames)
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(stream!);
            var content = await streamReader.ReadToEndAsync();
            sources.Add((resourceName.Replace(sourcesPrefix, ""), content));
        }

        var generatedSources = new SourceFileCollection();
        var generatedSourceResourceNames = resources.Where(x => x.StartsWith(generatedSourcesPrefix));
        foreach (var resourceName in generatedSourceResourceNames)
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(stream!);
            var content = await streamReader.ReadToEndAsync();
            generatedSources.Add((resourceName.Replace(sourcesPrefix, ""), content));
        }

        return (sources, generatedSources);
    }
}

