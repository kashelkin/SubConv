using SubConv.Data;

namespace SubConv.Transform
{
    public interface ISubtitleTransform
    {
        IEnumerable<SubtitleEntry> Transform(IEnumerable<SubtitleEntry> entries);
    }
}
