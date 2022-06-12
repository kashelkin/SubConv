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

        public static IEnumerable<IGrouping<TKey, TElement>> GroupConsequent<TElement, TKey>(this IEnumerable<TElement> data,
            Func<TElement, TKey> keySelector)
        {
            using var enumerator = data.GetEnumerator();
            if (!enumerator.MoveNext()) yield break;

            var elements = new List<TElement> { enumerator.Current };
            var key = keySelector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                if (!Equals(key, keySelector(enumerator.Current)))
                {
                    yield return new Group<TKey, TElement>(elements, key);
                    elements = new List<TElement>();
                    key = keySelector(enumerator.Current);
                }

                elements.Add(enumerator.Current);
            }

            yield return new Group<TKey, TElement>(elements, key);
        }
    }
}
