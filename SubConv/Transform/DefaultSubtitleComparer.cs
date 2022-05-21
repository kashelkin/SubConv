using SubConv.Data;

namespace SubConv.Transform
{
    public class DefaultSubtitleComparer : IComparer<SubtitleEntry>
    {
        public virtual int Compare(SubtitleEntry? x, SubtitleEntry? y)
        {
            if (x == null || y == null) return 0;

            return x.StartTime.CompareTo(y.StartTime);
        }
    }
}
