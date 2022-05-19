using SubConv.Data;
using SubConv.Processing.Internal;

namespace SubConv.Processing
{
    public static class SubtitleMerge
    {
        public static IEnumerable<SubtitleEntry> Merge(IEnumerable<SubtitleEntry> entries)
        {
            return MergedEntries(entries.ToList());
        }

        private static IEnumerable<SubtitleEntry> MergedEntries(IReadOnlyCollection<SubtitleEntry> entries)
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
                    merge.OrderBy(e => e.Content.StartsWith("[", StringComparison.Ordinal) ? 1 : 0)
                        .ThenBy(e => e.StartTime)
                        .Select(e => e.Content));

                yield return new SubtitleEntry(interval.Start, interval.End, content);
            }
        }

        private static IEnumerable<TimeInterval> ToIntervals(this IEnumerable<TimeSpan> dates)
        {
            using var data = dates.GetEnumerator();

            if (!data.MoveNext())
                yield break;

            var start = data.Current;

            while (data.MoveNext())
            {
                yield return new TimeInterval(start, data.Current);
                start = data.Current;
            }
        }

        private static IEnumerable<T> ToEnumerable<T>(params T[] items) => items;
    }
}
