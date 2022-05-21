using SubConv.Data;
using SubConv.Transform;
using System;
using Xunit;

namespace SubConvTest.Transform
{
    public class DefaultSubtitleComparerTest
    {
        [Fact]
        public void Compares_By_StartTime()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 14),
                "Entry1",
                "Default");
            var entry2 = new SubtitleEntry(
                new TimeSpan(1, 10, 13),
                new TimeSpan(1, 10, 14),
                "Entry2",
                "Names");
            var entry3 = new SubtitleEntry(
                new TimeSpan(1, 10, 13),
                new TimeSpan(1, 10, 22),
                "Entry3",
                "SmallNames");

            var sut = new DefaultSubtitleComparer();

            Assert.Equal(-1, sut.Compare(entry1, entry2));
            Assert.Equal(0, sut.Compare(entry2, entry3));
            Assert.Equal(1, sut.Compare(entry3, entry1));
        }
    }
}
