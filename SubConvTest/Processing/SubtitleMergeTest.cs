using SubConv.Data;
using SubConv.Processing;
using System;
using System.Collections.Generic;
using Xunit;

namespace SubConvTest.Processing
{
    public class SubtitleMergeTest
    {
        [Fact]
        public void Merge_Single_Entry()
        {
            var entry = new SubtitleEntry(TimeSpan.MinValue, TimeSpan.MaxValue, "Content");

            var result = SubtitleMerge.Merge(ToEnumerable(entry));

            Assert.Collection(result, e => e
                .WithStart(TimeSpan.MinValue)
                .WithEnd(TimeSpan.MaxValue)
                .WithContent("Content")
            );
        }

        [Fact]
        public void Two_Overlapped_Entries()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(0, 1, 0), 
                new TimeSpan(0, 5, 0),
                "Entry1");
            var entry2 = new SubtitleEntry(
                new TimeSpan(0, 4, 0),
                new TimeSpan(0, 7, 0),
                "Entry2");

            var result = SubtitleMerge.Merge(ToEnumerable(entry1, entry2));

            Assert.Collection(result, e => e
                    .WithStart(0, 1, 0)
                    .WithEnd(0, 4, 0)
                    .WithContent("Entry1"),
                e => e
                    .WithStart(0, 4, 0)
                    .WithEnd(0, 5, 0)
                    .WithContent("Entry1\nEntry2".EnvNewLine()),
                e => e
                    .WithStart(0, 5, 0)
                    .WithEnd(0, 7, 0)
                    .WithContent("Entry2"));
        }

        [Fact]
        public void Two_Independent_Entries()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(0, 1, 0), 
                new TimeSpan(0, 3, 0),
                "Entry1");
            var entry2 = new SubtitleEntry(
                new TimeSpan(0, 4, 0),
                new TimeSpan(0, 7, 0),
                "Entry2");

            var result = SubtitleMerge.Merge(ToEnumerable(entry1, entry2));

            Assert.Collection(result, e => e
                    .WithStart(0, 1, 0)
                    .WithEnd(0, 3, 0)
                    .WithContent("Entry1"),
                e => e
                    .WithStart(0, 4, 0)
                    .WithEnd(0, 7, 0)
                    .WithContent("Entry2"));
        }

        [Fact]
        public void Two_Adjacent_Entries()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(0, 1, 0), 
                new TimeSpan(0, 4, 0),
                "Entry1");
            var entry2 = new SubtitleEntry(
                new TimeSpan(0, 4, 0),
                new TimeSpan(0, 7, 0),
                "Entry2");

            var result = SubtitleMerge.Merge(ToEnumerable(entry1, entry2));

            Assert.Collection(result, e => e
                    .WithStart(0, 1, 0)
                    .WithEnd(0, 4, 0)
                    .WithContent("Entry1"),
                e => e
                    .WithStart(0, 4, 0)
                    .WithEnd(0, 7, 0)
                    .WithContent("Entry2"));
        }

        [Fact]
        public void Two_Simultaneous_Entries()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(0, 1, 0), 
                new TimeSpan(0, 4, 0),
                "Entry1");
            var entry2 = new SubtitleEntry(
                new TimeSpan(0, 1, 0),
                new TimeSpan(0, 4, 0),
                "Entry2");

            var result = SubtitleMerge.Merge(ToEnumerable(entry1, entry2));

            Assert.Collection(result, e => e
                .WithStart(0, 1, 0)
                .WithEnd(0, 4, 0)
                .WithContent("Entry1\nEntry2".EnvNewLine()));
        }

        [Fact]
        public void Reverse_Entries_Sorted_Properly()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(0, 1, 0), 
                new TimeSpan(0, 5, 0),
                "Entry1");
            var entry2 = new SubtitleEntry(
                new TimeSpan(0, 4, 0),
                new TimeSpan(0, 7, 0),
                "Entry2");

            var result = SubtitleMerge.Merge(ToEnumerable(entry2, entry1));

            Assert.Collection(result, e => e
                    .WithStart(0, 1, 0)
                    .WithEnd(0, 4, 0)
                    .WithContent("Entry1"),
                e => e
                    .WithStart(0, 4, 0)
                    .WithEnd(0, 5, 0)
                    .WithContent("Entry1\nEntry2".EnvNewLine()),
                e => e
                    .WithStart(0, 5, 0)
                    .WithEnd(0, 7, 0)
                    .WithContent("Entry2"));
        }

        [Fact]
        public void Title_Always_Below()
        {
            var entry1 = new SubtitleEntry(
                new TimeSpan(0, 1, 0), 
                new TimeSpan(0, 7, 0),
                "[Title Entry]");
            var entry2 = new SubtitleEntry(
                new TimeSpan(0, 4, 0),
                new TimeSpan(0, 7, 0),
                "Entry2");

            var result = SubtitleMerge.Merge(ToEnumerable(entry1, entry2));

            Assert.Collection(result, e => e
                    .WithStart(0, 1, 0)
                    .WithEnd(0, 4, 0)
                    .WithContent("[Title Entry]"),
                e => e
                    .WithStart(0, 4, 0)
                    .WithEnd(0, 7, 0)
                    .WithContent("Entry2\n[Title Entry]".EnvNewLine()));
        }

        [Fact]
        public void Zero_Entries()
        {
            var result = SubtitleMerge.Merge(new SubtitleEntry[] { });

            Assert.Empty(result);
        }

        private static IEnumerable<T> ToEnumerable<T>(params T[] data) => data;
    }
}
