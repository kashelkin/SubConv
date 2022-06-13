using SubConv.Data;
using System.Collections.Generic;

namespace SubConv.Transform
{
    public interface ISubtitleTransform
    {
        IEnumerable<SubtitleEntry> Transform(IEnumerable<SubtitleEntry> entries);
    }
}
