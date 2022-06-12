using SubConv.Data;
using SubConv.Transform;
using System;
using Xunit;

namespace SubConvTest.Transform
{
    public class TransformChainTest : BaseTransformTest
    {
        [Fact]
        public void Empty_Chain_Does_Not_Transform()
        {
            var entry = new SubtitleEntry(
                new TimeSpan(2, 10, 12),
                new TimeSpan(2, 10, 15),
                "Entry",
                "Default");

            var sut = new TransformChain(Array.Empty<ISubtitleTransform>());
            var result = sut.Transform(ToEnumerable(entry));

            Assert.Collection(result, e => e
                .WithStart(2, 10, 12)
                .WithEnd(2, 10, 15)
                .WithContent("Entry")
                .WithStyle("Default"));
        }

        [Fact]
        public void Transforms_Applied_Consequently()
        {
            var entry = new SubtitleEntry(
                new TimeSpan(2, 10, 12),
                new TimeSpan(2, 10, 15),
                "Entry",
                "Default");

            var transforms = new[]
            {
                new WrapContentTransform("Default", "[", "]"),
                new WrapContentTransform("Default", "{", "}")
            };

            var sut = new TransformChain(transforms);
            var result = sut.Transform(ToEnumerable(entry));

            Assert.Collection(result, e => e
                .WithStart(2, 10, 12)
                .WithEnd(2, 10, 15)
                .WithContent("{[Entry]}")
                .WithStyle("Default"));
        }
    }
}
