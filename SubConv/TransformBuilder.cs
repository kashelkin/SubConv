using SubConv.Transform;

namespace SubConv
{
    internal class TransformBuilder
    {
        private readonly Dictionary<string, List<TransformData>> _transforms = new();

        public TransformBuilder RegisterFor(string name, Func<IReadOnlyList<string>, bool> condition, Func<IReadOnlyList<string>, ISubtitleTransform> transform)
        {
            if (!_transforms.ContainsKey(name))
                _transforms.Add(name, new List<TransformData>());
            _transforms[name].Add(new TransformData(condition, transform));
            return this;
        }

        public TransformBuilder Register(string name, Func<IReadOnlyList<string>, ISubtitleTransform> transform) =>
            RegisterFor(name, _ => true, transform);

        public ISubtitleTransform Build(IEnumerable<TransformOptions> options)
        {
            var transforms = options
                .Select(o => _transforms[o.Name]
                    .Where(x => x.Condition(o.Params))
                    .Select(x => x.Transform(o.Params))
                    .First())
                .ToList();

            return new TransformChain(transforms);
        }

        private class TransformData
        {
            public Func<IReadOnlyList<string>, bool> Condition { get; }
            public Func<IReadOnlyList<string>, ISubtitleTransform> Transform { get; }

            public TransformData(Func<IReadOnlyList<string>, bool> condition, Func<IReadOnlyList<string>, ISubtitleTransform> transform)
            {
                Condition = condition;
                Transform = transform;
            }
        }
    }
}
