using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace ServiceConstruct
{

    [Generator]
    public class Generator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Code generation goes here
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}

