using Microsoft;
using Microsoft.CodeAnalysis.Text;
using System.Text;
namespace ServiceConstruct.Tests;

using VerifyCS = CSharpSourceGeneratorVerifier<ServiceConstruct.Generator>;

public class SanityTest
{
    [Fact]
    public async Task Test()
    {
        var code = "";
        var test = new VerifyCS.Test
        {
            TestState =
            {
                Sources = { code },
                GeneratedSources =
                {
                },
            },
        };
        await test.RunAsync();
    }
}
