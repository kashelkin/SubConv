using CommandLine;
using SubConv;
using SubConv.Providers.Ass;
using SubConv.Providers.Srt;

var options = Parser.Default.ParseArguments<Options>(args)
    .WithParsed(o => Convert(o.InputPath, o.OutputPath));

void Convert(string inputPath, string? outputPath)
{
    foreach (var file in PathParser.GetFiles(inputPath))
    {
        using var sr = new StreamReader(file);
        using var sw = new StreamWriter(PathParser.GenerateOutputFileName(file, outputPath, "srt"));

        SrtWriter.Write(sw, AssReader.Read(sr));
    }
}