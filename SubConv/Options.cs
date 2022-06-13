using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace SubConv
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Options is instantiated by CommandLineParser")]
    internal class Options
    {
        [Value(0, Required = true, MetaName = "InputPath", HelpText = "Input path")]
        public string InputPath { get; }

        [Value(1, Required = false, MetaName = "OutputPath", HelpText = "Output path")]
        public string? OutputPath { get; }

        [Option('t', HelpText = "Transform")]
        public IReadOnlyList<TransformOptions> Transforms { get; }

        public Options(string inputPath, string? outputPath, IReadOnlyList<TransformOptions> transforms)
        {
            InputPath = inputPath;
            OutputPath = outputPath;
            Transforms = transforms;
        }
    }
}
