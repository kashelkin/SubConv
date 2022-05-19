using CommandLine;

namespace SubConv
{
    public class Options
    {
        [Value(0, Required = true, MetaName = "InputPath", HelpText = "Input path")]
        public string InputPath { get; }

        [Value(1, Required = false, MetaName = "OutputPath", HelpText = "Output path")]
        public string? OutputPath { get; }

        public Options(string inputPath, string? outputPath)
        {
            InputPath = inputPath;
            OutputPath = outputPath;
        }
    }
}
