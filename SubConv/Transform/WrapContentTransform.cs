using System.Text;
using SubConv.Data;

namespace SubConv.Transform
{
    public class WrapContentTransform : ISubtitleTransform
    {
        private readonly IReadOnlyDictionary<string, int> _styles;
        private readonly string _start;
        private readonly string _end;

        public WrapContentTransform(string styles, string start, string end)
        {
            if (styles == null) throw new ArgumentNullException(nameof(styles));

            _styles = styles.Split(',')
                .ToDictionary(x => x, x => 0);

            _start = start;
            _end = end;
        }

        public IEnumerable<SubtitleEntry> Transform(IEnumerable<SubtitleEntry> entries) => entries.Select(EncloseEntry);

        private SubtitleEntry EncloseEntry(SubtitleEntry entry)
        {
            if (!ShouldTransform(entry)) return entry;
            if (entry.Content.StartsWith(_start, StringComparison.Ordinal) &&
                entry.Content.EndsWith(_end, StringComparison.Ordinal))
                return entry;

            var sb = new StringBuilder();
            if (!entry.Content.StartsWith(_start, StringComparison.Ordinal))
                sb.Append(_start);
            sb.Append(entry.Content);
            if (!entry.Content.EndsWith(_end, StringComparison.Ordinal))
                sb.Append(_end);

            return entry.WithContent(sb.ToString());
        }

        private bool ShouldTransform(SubtitleEntry entry)
        {
            if (_styles.ContainsKey("*")) return true;
            if (entry.StyleName == null) return false;
            return _styles.ContainsKey(entry.StyleName);
        }
    }
}
