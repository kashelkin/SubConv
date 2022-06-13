using SubConv.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SubConv.Providers.Ass;

public static class AssReader
{
    public static IEnumerable<SubtitleEntry> Read(TextReader reader)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));

        while (reader.ReadLine() is { } line && !line.StartsWith("[Events]", StringComparison.Ordinal))
        { }

        Dictionary<string, int>? format = null;

        while (reader.ReadLine() is { } line)
        {
            if (!line.StartsWith("Format:", StringComparison.Ordinal)) continue;
            format = CreateFormat(line);
            break;
        }

        while (reader.ReadLine() is { } line)
        {
            if (line.StartsWith("Dialogue:", StringComparison.Ordinal))
#pragma warning disable CS8604 // Possible null reference argument.
                // At this point format will have value.
                yield return CreateEntry(line, format);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }

    private static Dictionary<string, int> CreateFormat(string line)
    {
        return line.Split(':', 2, StringSplitOptions.TrimEntries)[1]
            .Split(',', StringSplitOptions.TrimEntries)
            .Select((x, i) => new { Key = x, Value = i })
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private static SubtitleEntry CreateEntry(string line, IReadOnlyDictionary<string, int> format)
    {
        var splitLine = line.Split(':', 2, StringSplitOptions.TrimEntries)[1]
            .Split(',', format.Count, StringSplitOptions.TrimEntries);

        var startTime = ParseTimeSpan(splitLine[format["Start"]]);
        var endTime = ParseTimeSpan(splitLine[format["End"]]);
        var content = RemoveAssStyle(splitLine[format["Text"]]);
        var style = splitLine[format["Style"]];
        var position = ParsePosition(splitLine[format["Text"]]);

        return new SubtitleEntry(startTime, endTime, content, style, position);
    }

    private static string RemoveAssStyle(string text) =>
        Regex.Replace(text, @"\{[^}]*\}", "")
            .Replace("\\N", Environment.NewLine, StringComparison.Ordinal);

    private static TimeSpan ParseTimeSpan(string value) =>
        TimeSpan.ParseExact(value, @"h\:mm\:ss\.ff", CultureInfo.InvariantCulture);

    private static Position? ParsePosition(string value)
    {
        var matches = Regex.Matches(value, @"{.*\\(pos\((?<x>[\d\.]+),(?<y>[\d\.]+))\).*}");
        if (matches.Count == 0) return null;
        var match = matches.First();
        var x = decimal.Parse(match.Groups["x"].Value, CultureInfo.InvariantCulture);
        var y = decimal.Parse(match.Groups["y"].Value, CultureInfo.InvariantCulture);

        return new Position(x, y);
    }
}