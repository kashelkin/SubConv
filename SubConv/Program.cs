using CommandLine;
using CommandLine.Text;
using SubConv;
using SubConv.Providers.Ass;
using SubConv.Providers.Srt;
using SubConv.Transform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

var transformBuilder = GetTransformBuilder();

using var parser = new Parser(with => with.HelpWriter = null);
var parserResult = parser.ParseArguments<Options>(args);
parserResult
    .WithParsed(o => Convert(o.InputPath, o.OutputPath, transformBuilder.Build(o.Transforms)))
    .WithNotParsed(errs => Console.WriteLine(GetHelpText(parserResult, errs)));


TransformBuilder GetTransformBuilder()
{
    var transformBuilder = new TransformBuilder();
    transformBuilder.RegisterFor("c",
        o => o.Count == 0,
        _ => new ContentCleanupTransform());
    transformBuilder.RegisterFor("k",
        o => o.Count == 0,
        _ => new KaraokeTransform());
    transformBuilder.RegisterFor("k",
        o => o.Count == 1,
        o => new KaraokeTransform(o[0]));
    transformBuilder.RegisterFor("m",
        o=> o.Count == 0,
        _ => new SortAndMergeTransform());
    transformBuilder.RegisterFor("m",
        o => o.Count > 0,
        o => new SortAndMergeTransform(new StyleSubtitleComparer(o)));
    transformBuilder.RegisterFor("w",
        o => o.Count == 3,
        o => new WrapContentTransform(o[0], o[1], o[2]));

    return transformBuilder;
}

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

HelpText GetHelpText<T>(ParserResult<T> result, IEnumerable<Error> errs)
{
    if (errs.IsVersion())
        return HelpText.AutoBuild(result);

    return HelpText.AutoBuild(result, h =>
    {
        h.AddNewLineBetweenHelpSections = true;
        h.AdditionalNewLineAfterOption = false;
        h.MaximumDisplayWidth = 100;
        h.AddPreOptionsLines(new[] {
            "USAGE:",
            "  subconv <InputPath>",
            "  subconv <InputPath> -t [Transforms]",
            "  subconv <InputPath> <OutputPath> -t [Transforms]",
            "",
            "TRANSFORMS:",
            "  c                           Cleanup. Removes multiple spaces, empty lines and empty subtitle",
            "                              entries.",
            "  w:[Styles];[Start];[End]    Wrap. Wraps subtitle entries of certain styles in [Start] and [End]", 
            "                              strings. If you want to wrap entries of all styles use *.",
            "  m:[Styles1];[Styles2]...    Merge. Merges overlapping in time subtitle entries to single entry.",
            "                              Vertical order of merged entries is specified in list of [Styles]. Merge must be the last transform because merged entries have no style information.",
            "",
            "EXAMPLES:",
            "  subconv *.ass -t c w:Names,SmallNames;[;] w:smallfont;{;} m:Names,SmallNames;*;smallfont",
            "",
            "    - cleanups subtitle entries",
            "    - wraps subtitle entries of styles 'Names' and 'SmallNames' in [];",
            "    - wraps subtitle entries of style 'smallfont' in {};",
            "    - merges subtitle entries. Vertical order:",
            "          - 'Names', 'SmallNames'",
            "          - all other styles",
            "          - 'smallfont'."
        });
        return HelpText.DefaultParsingErrorsHandler(result, h);
    }, e => e);
}
