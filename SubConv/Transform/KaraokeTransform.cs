using SubConv.Data;
using SubConv.Transform.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SubConv.Transform;

public class KaraokeTransform : ISubtitleTransform
{
    private readonly IReadOnlyList<string>? _styles;

    public KaraokeTransform()
    {}

    public KaraokeTransform(IReadOnlyList<string> styles)
    {
        _styles = styles;
    }

    public IEnumerable<SubtitleEntry> Transform(IEnumerable<SubtitleEntry> entries)
    {
        var allEntries = entries.ToList();

        var transform = _styles != null 
            ? allEntries.Where(e => _styles.Any(s => s == e.StyleName)) 
            : allEntries;

        var ignore = _styles != null
            ? allEntries.Where(e => _styles.All(s => s != e.StyleName))
            : Array.Empty<SubtitleEntry>();

        return transform.GroupBy(e => $"{e.StyleName},{e.EndTime}")
            .Select(Merge)
            .Concat(ignore);
    }

    private static SubtitleEntry Merge(IGrouping<string, SubtitleEntry> group)
    {
        var positions = group.GroupBy(e => e.Position)
            .Select(g => new { StartTime = g.Min(e => e.StartTime), Entries = g.ToList() })
            .ToList();

        var length = positions
            .SelectMany(p => p.Entries.GroupConsequent(e => e.Content))
            .Select(g => g.Count())
            .Min();

        var content = string.Concat(positions.OrderBy(p => p.StartTime)
            .SelectMany(p => p.Entries.GroupConsequent(e => e.Content)
                .SelectMany(g => Enumerable.Repeat(string.IsNullOrEmpty(g.Key) ? " " : g.Key, g.Count() / length))));

        return new SubtitleEntry(
            startTime: group.Select(e => e.StartTime).Min(),
            endTime: group.Select(e => e.EndTime).First(),
            content: CheckRepeated(content),
            styleName: group.Select(e => e.StyleName).First());
    }

    private static string CheckRepeated(string content)
    {
        var match = Regex.Match(content, @"(.+?)\1{1,}$");
        return match.Success 
            ? match.Groups[1].Value 
            : content;
    }
}