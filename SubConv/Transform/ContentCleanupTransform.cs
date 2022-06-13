using System;
using SubConv.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SubConv.Transform;

public class ContentCleanupTransform : ISubtitleTransform
{
    public IEnumerable<SubtitleEntry> Transform(IEnumerable<SubtitleEntry> entries)
    {
        return RemoveEmpty(TrimSpaces(entries));
    }

    private static IEnumerable<SubtitleEntry> TrimSpaces(IEnumerable<SubtitleEntry> entries)
    {
        return entries.Select(e => e.WithContent(
            string.Join(Environment.NewLine, Regex.Replace(e.Content, " {2,}", " ")
                .Split(Environment.NewLine)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim()))));
    }

    private static IEnumerable<SubtitleEntry> RemoveEmpty(IEnumerable<SubtitleEntry> entries)
    {
        return entries.Where(e => !string.IsNullOrEmpty(e.Content));
    }
}