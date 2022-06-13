using SubConv.Data;
using SubConv.Transform.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubConv.Transform;

public class SortAndMergeTransform : ISubtitleTransform
{
    private readonly IComparer<SubtitleEntry> _comparer;

    public SortAndMergeTransform()
    {
        _comparer = new DefaultSubtitleComparer();
    }

    public SortAndMergeTransform(IComparer<SubtitleEntry> comparer)
    {
        _comparer = comparer;
    }

    public IEnumerable<SubtitleEntry> Transform(IEnumerable<SubtitleEntry> entries)
    {
        return MergedEntries(entries.ToList());
    }

    private IEnumerable<SubtitleEntry> MergedEntries(IReadOnlyCollection<SubtitleEntry> entries)
    {
        var intervals = entries
            .SelectMany(e => ToEnumerable(e.StartTime, e.EndTime))
            .OrderBy(x => x)
            .Distinct()
            .ToIntervals();

        foreach (var interval in intervals)
        {
            var merge = entries.Where(e => interval.Intersects(e.StartTime, e.EndTime));

            if (!merge.Any()) continue;

            var content = string.Join(Environment.NewLine,
                merge.OrderBy(e => e, _comparer)
                    .Select(e => e.Content));

            yield return new SubtitleEntry(interval.Start, interval.End, content);
        }
    }

    private static IEnumerable<T> ToEnumerable<T>(params T[] items) => items;
}