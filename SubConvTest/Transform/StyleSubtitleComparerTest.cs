using SubConv.Data;
using SubConv.Transform;
using System;
using Xunit;

namespace SubConvTest.Transform
{
    public class StyleSubtitleComparerTest
    {
        [Fact]
        public void Compares_By_StyleName()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 20),
                "Entry1",
                "Default");
            var entry2 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 20),
                "Entry2",
                "Names");
            var entry3 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 20),
                "Entry2",
                "Names");

            var sut = new StyleSubtitleComparer(new[] { "Default", "Names" });

            Assert.Equal(-1, sut.Compare(entry1, entry2));
            Assert.Equal(0, sut.Compare(entry2, entry3));
            Assert.Equal(1, sut.Compare(entry3, entry1));
        }

        [Fact]
        public void When_Style_Order_Equal_Compares_By_StartTime()
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
                "Default");
            var entry3 = new SubtitleEntry(
                new TimeSpan(1, 10, 13),
                new TimeSpan(1, 10, 22),
                "Entry3",
                "Default");

            var sut = new StyleSubtitleComparer(new[] { "Default", "Names" });

            Assert.Equal(-1, sut.Compare(entry1, entry2));
            Assert.Equal(0, sut.Compare(entry2, entry3));
            Assert.Equal(1, sut.Compare(entry3, entry1));
        }

        [Fact]
        public void Multiple_Styles_With_Same_Order()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 14),
                "Entry1",
                "Default");
            var entry2 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 14),
                "Entry2",
                "Names");
            var entry3 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 14),
                "Entry3",
                "SmallNames");

            var sut = new StyleSubtitleComparer(new[] { "Default", "Names,SmallNames" });

            Assert.Equal(-1, sut.Compare(entry1, entry2));
            Assert.Equal(0, sut.Compare(entry2, entry3));
            Assert.Equal(1, sut.Compare(entry3, entry1));
        }

        [Fact]
        public void Using_Wildcard()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 14),
                "Entry1",
                "Default");
            var entry2 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 14),
                "Entry2",
                "Names");
            var entry3 = new SubtitleEntry(
                new TimeSpan(1, 10, 12),
                new TimeSpan(1, 10, 14),
                "Entry3",
                "SmallNames");


            var sut = new StyleSubtitleComparer(new[] { "Default", "*" });

            Assert.Equal(-1, sut.Compare(entry1, entry2));
            Assert.Equal(0, sut.Compare(entry2, entry3));
            Assert.Equal(1, sut.Compare(entry3, entry1));
        }
    }
}
