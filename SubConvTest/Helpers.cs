using SubConv.Data;
using System;
using Xunit;

namespace SubConvTest
{
    internal static class Helpers
    {
        public static SubtitleEntry WithStart(this SubtitleEntry entry, int hours, int minutes, int seconds)
        {
            Assert.Equal(new TimeSpan(hours, minutes, seconds), entry.StartTime);
            return entry;
        }

        public static SubtitleEntry WithStart(this SubtitleEntry entry, TimeSpan timeSpan)
        {
            Assert.Equal(timeSpan, entry.StartTime);
            return entry;
        }

        public static SubtitleEntry WithEnd(this SubtitleEntry entry, int hours, int minutes, int seconds)
        {
            Assert.Equal(new TimeSpan(hours, minutes, seconds), entry.EndTime);
            return entry;
        }

        public static SubtitleEntry WithEnd(this SubtitleEntry entry, TimeSpan timeSpan)
        {
            Assert.Equal(timeSpan, entry.EndTime);
            return entry;
        }

        public static SubtitleEntry WithContent(this SubtitleEntry entry, string content)
        {
            Assert.Equal(content, entry.Content);
            return entry;
        }

        public static string EnvNewLine(this string it) => it.Replace("\n", Environment.NewLine);
    }
}
