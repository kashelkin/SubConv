namespace SubConv.Transform.Internal
{
    internal static class Helpers
    {
        public static IEnumerable<TimeInterval> ToIntervals(this IEnumerable<TimeSpan> dates)
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
    }
}
