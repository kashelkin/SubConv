using SubConv.Data;
using System.Collections.Generic;

namespace SubConv.Transform
{
    public class TransformChain : ISubtitleTransform
    {
        private readonly IReadOnlyList<ISubtitleTransform> _process;

        public TransformChain(IReadOnlyList<ISubtitleTransform> process)
        {
            _process = process;
        }

        public IEnumerable<SubtitleEntry> Transform(IEnumerable<SubtitleEntry> entries)
        {
            if (_process.Count == 0)
                return entries;

            var result = _process[0].Transform(entries);

            for (var i = 1; i < _process.Count; i++)
            {
                result = _process[i].Transform(result);
            }

            return result;
        }
    }
}
