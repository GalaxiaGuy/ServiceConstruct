namespace ServiceConstruct.Tests;

using VerifyCS = CSharpSourceGeneratorVerifier<ServiceConstruct.Generator>;

public class Test1 : BaseTest
{
    public Test1()
    {
    }

    [Fact]
    public async Task Test()
    {
        (var sources, var generatedSources) = await LoadAsync("Test1");
        var test = new VerifyCS.Test();
        test.TestState.Sources.AddRange(sources);
        test.TestState.GeneratedSources.AddRange(generatedSources);
        await test.RunAsync();
    }
}

