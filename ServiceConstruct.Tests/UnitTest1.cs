using Microsoft;
using Microsoft.CodeAnalysis.Text;
using System.Text;
namespace ServiceConstruct.Tests;

using VerifyCS = CSharpSourceGeneratorVerifier<ServiceConstruct.Generator>;

public class UnitTest1
{
    [Fact]
    public async Task SanityTest()
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
