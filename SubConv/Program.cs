using CommandLine;
using SubConv;
using SubConv.Providers.Ass;
using SubConv.Providers.Srt;
using SubConv.Transform;
using System.Text;

var transformBuilder = new TransformBuilder();
transformBuilder.RegisterFor("merge",
    o=> o.Count == 0,
    _ => new SortAndMergeTransform());
transformBuilder.RegisterFor("merge",
    o => o.Count > 0,
    o => new SortAndMergeTransform(new StyleSubtitleComparer(o)));
transformBuilder.RegisterFor("wrap",
    o => o.Count == 3,
    o => new WrapContentTransform(o[0], o[1], o[2]));

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(o => Convert(o.InputPath, o.OutputPath, transformBuilder.Build(o.Transforms)));

void Convert(string inputPath, string? outputPath, ISubtitleTransform transform)
{
    foreach (var file in PathParser.GetFiles(inputPath))
    {
        using var sr = new StreamReader(file);
        using var sw = new StreamWriter(
            PathParser.GenerateOutputFileName(file, outputPath, "srt"),
            false,
            Encoding.UTF8);

        SrtWriter.Write(sw, transform.Transform(AssReader.Read(sr)));
    }
}