using SubConv.Data;
using System.Collections.Generic;
using System.Linq;

namespace SubConv.Transform
{
    public class StyleSubtitleComparer : DefaultSubtitleComparer
    {
        private readonly IReadOnlyDictionary<string, int> _styleOrder;

        public StyleSubtitleComparer(IEnumerable<string> styles)
        {
            _styleOrder = styles.Select((x, i) => new { Styles = x, Order = i })
                .SelectMany(x => x.Styles
                    .Split(',')
                    .Select(y => new { Style = y, x.Order }))
                .ToDictionary(x => x.Style, x => x.Order);
        }

        public override int Compare(SubtitleEntry? x, SubtitleEntry? y)
        {
            if (x == null || y == null) return 0;

            var result = GetStyleOrder(x.StyleName).CompareTo(GetStyleOrder(y.StyleName));

            return result != 0
                ? result
                : base.Compare(x, y);
        }

        private int GetStyleOrder(string? style)
        {
            if (style == null) return _styleOrder["*"];

            if (!_styleOrder.TryGetValue(style, out var result))
                result = _styleOrder["*"];

            return result;
        }
    }
}
