using SubConv.Data;
using System;
using System.IO;
using Xunit;

namespace SubConvTest;

internal static class Helpers
{
    public static SubtitleEntry HasStart(this SubtitleEntry entry, int hours, int minutes, int seconds)
    {
        Assert.Equal(new TimeSpan(hours, minutes, seconds), entry.StartTime);
        return entry;
    }

    public static SubtitleEntry HasStart(this SubtitleEntry entry, TimeSpan timeSpan)
    {
        Assert.Equal(timeSpan, entry.StartTime);
        return entry;
    }

    public static SubtitleEntry HasStart(this SubtitleEntry entry, int hours, int minutes, int seconds,
        int milliseconds)
    {
        Assert.Equal(new TimeSpan(0, hours, minutes ,seconds ,milliseconds), entry.StartTime);
        return entry;
    }

    public static SubtitleEntry HasEnd(this SubtitleEntry entry, int hours, int minutes, int seconds)
    {
        Assert.Equal(new TimeSpan(hours, minutes, seconds), entry.EndTime);
        return entry;
    }

    public static SubtitleEntry HasEnd(this SubtitleEntry entry, TimeSpan timeSpan)
    {
        Assert.Equal(timeSpan, entry.EndTime);
        return entry;
    }

    public static SubtitleEntry HasEnd(this SubtitleEntry entry, int hours, int minutes, int seconds,
        int milliseconds)
    {
        Assert.Equal(new TimeSpan(0, hours, minutes ,seconds ,milliseconds), entry.EndTime);
        return entry;
    }

    public static SubtitleEntry HasContent(this SubtitleEntry entry, string content)
    {
        Assert.Equal(content, entry.Content);
        return entry;
    }

    public static SubtitleEntry HasStyle(this SubtitleEntry entry, string? style)
    {
        Assert.Equal(style, entry.StyleName);
        return entry;
    }

    public static SubtitleEntry HasPosition(this SubtitleEntry entry, decimal x, decimal y)
    {
        Assert.NotNull(entry.Position);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        // We have NotNull assertion before this code.
        Assert.Equal(x, entry.Position.X);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        Assert.Equal(y, entry.Position.Y);
        return entry;
    }

    public static string EnvNewLine(this string it) => it.Replace("\n", Environment.NewLine, StringComparison.InvariantCulture);

    public static string? FixSlash(this string? it) => it?.Replace('\\', Path.DirectorySeparatorChar);
}