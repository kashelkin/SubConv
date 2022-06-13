using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SubConv.Transform.Internal;

internal class Group<TKey, TElement> : IGrouping<TKey, TElement>
{
    private readonly List<TElement> _elements;

    public TKey Key { get; }

    public IEnumerator<TElement> GetEnumerator() => _elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Group(List<TElement> elements, TKey key)
    {
        _elements = elements;
        Key = key;
    }
}