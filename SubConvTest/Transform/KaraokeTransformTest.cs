using SubConv.Data;
using SubConv.Transform;
using System;
using System.Linq;
using Xunit;

namespace SubConvTest.Transform;

public class KaraokeTransformTest : BaseTransformTest
{
    [Fact]
    public void Single_Letter_Transform()
    {
        var sut = new KaraokeTransform();
        var karaoke = ToEnumerable(
            new SubtitleEntry(new TimeSpan(0, 1, 0), new TimeSpan(0, 1, 10), "a"),
            new SubtitleEntry(new TimeSpan(0, 1, 3), new TimeSpan(0, 1, 10), "n"),
            new SubtitleEntry(new TimeSpan(0, 1, 6), new TimeSpan(0, 1, 10), "d"));

        var result = sut.Transform(karaoke);

        Assert.Collection(result, e => e
            .HasStart(0, 1, 0)
            .HasEnd(0, 1, 10)
            .HasContent("and"));
    }

    [Fact]
    public void Multiple_Letter_Transform()
    {
        var sut = new KaraokeTransform();
        var karaoke = ToEnumerable(
            new SubtitleEntry(new TimeSpan(0, 1, 0), new TimeSpan(0, 1, 10), "a"),
            new SubtitleEntry(new TimeSpan(0, 1, 1), new TimeSpan(0, 1, 10), "a"),
            new SubtitleEntry(new TimeSpan(0, 1, 2), new TimeSpan(0, 1, 10), "a"),
            new SubtitleEntry(new TimeSpan(0, 1, 3), new TimeSpan(0, 1, 10), "n"),
            new SubtitleEntry(new TimeSpan(0, 1, 4), new TimeSpan(0, 1, 10), "n"),
            new SubtitleEntry(new TimeSpan(0, 1, 5), new TimeSpan(0, 1, 10), "n"),
            new SubtitleEntry(new TimeSpan(0, 1, 6), new TimeSpan(0, 1, 10), "d"),
            new SubtitleEntry(new TimeSpan(0, 1, 7), new TimeSpan(0, 1, 10), "d"),
            new SubtitleEntry(new TimeSpan(0, 1, 8), new TimeSpan(0, 1, 10), "d"));

        var result = sut.Transform(karaoke);

        Assert.Collection(result, e => e
            .HasStart(0, 1, 0)
            .HasEnd(0, 1, 10)
            .HasContent("and"));
    }

    [Fact]
    public void Multiple_Style_Transform()
    {
        var sut = new KaraokeTransform();
        var karaoke = ToEnumerable(
            new SubtitleEntry(new TimeSpan(0, 1, 0), new TimeSpan(0, 1, 10), "a", "karaoke"),
            new SubtitleEntry(new TimeSpan(0, 1, 3), new TimeSpan(0, 1, 10), "n", "karaoke"),
            new SubtitleEntry(new TimeSpan(0, 1, 6), new TimeSpan(0, 1, 10), "d", "karaoke"),
            new SubtitleEntry(new TimeSpan(0, 1, 1), new TimeSpan(0, 1, 10), "n", "song"),
            new SubtitleEntry(new TimeSpan(0, 1, 4), new TimeSpan(0, 1, 10), "o", "song"),
            new SubtitleEntry(new TimeSpan(0, 1, 7), new TimeSpan(0, 1, 10), "w", "song"));

        var result = sut.Transform(karaoke).OrderBy(e => e.StartTime);

        Assert.Collection(result, e => e
                .HasStart(0, 1, 0)
                .HasEnd(0, 1, 10)
                .HasContent("and")
                .HasStyle("karaoke"),
            e => e
                .HasStart(0, 1, 1)
                .HasEnd(0, 1, 10)
                .HasContent("now")
                .HasStyle("song"));
    }

    [Fact]
    public void Treat_Empty_String_As_Space()
    {
        var sut = new KaraokeTransform();
        var karaoke = ToEnumerable(
            new SubtitleEntry(new TimeSpan(0, 1, 0), new TimeSpan(0, 1, 10), "a"),
            new SubtitleEntry(new TimeSpan(0, 1, 3), new TimeSpan(0, 1, 10), ""),
            new SubtitleEntry(new TimeSpan(0, 1, 6), new TimeSpan(0, 1, 10), "b"));

        var result = sut.Transform(karaoke);

        Assert.Collection(result, e => e
            .HasStart(0, 1, 0)
            .HasEnd(0, 1, 10)
            .HasContent("a b"));
    }

    [Fact]
    public void Intersecting_Entries()
    {
        var sut = new KaraokeTransform();
        var karaoke = ToEnumerable(
            new SubtitleEntry(new TimeSpan(0, 1, 0), new TimeSpan(0, 1, 10), "a", null, new Position(0, 0)),
            new SubtitleEntry(new TimeSpan(0, 1, 1), new TimeSpan(0, 1, 10), "a", null, new Position(0, 0)),
            new SubtitleEntry(new TimeSpan(0, 1, 2), new TimeSpan(0, 1, 10), "a", null, new Position(0, 0)),
            new SubtitleEntry(new TimeSpan(0, 1, 3), new TimeSpan(0, 1, 10), "a", null, new Position(0, 0)),
            new SubtitleEntry(new TimeSpan(0, 1, 2), new TimeSpan(0, 1, 10), "n", null, new Position(0, 10)),
            new SubtitleEntry(new TimeSpan(0, 1, 3), new TimeSpan(0, 1, 10), "n", null, new Position(0, 10)),
            new SubtitleEntry(new TimeSpan(0, 1, 4), new TimeSpan(0, 1, 10), "n", null, new Position(0, 10)),
            new SubtitleEntry(new TimeSpan(0, 1, 5), new TimeSpan(0, 1, 10), "n", null, new Position(0, 10)),
            new SubtitleEntry(new TimeSpan(0, 1, 6), new TimeSpan(0, 1, 10), "n", null, new Position(0, 10)),
            new SubtitleEntry(new TimeSpan(0, 1, 5), new TimeSpan(0, 1, 10), "d", null, new Position(0, 20)),
            new SubtitleEntry(new TimeSpan(0, 1, 6), new TimeSpan(0, 1, 10), "d", null, new Position(0, 20)),
            new SubtitleEntry(new TimeSpan(0, 1, 7), new TimeSpan(0, 1, 10), "d", null, new Position(0, 20)),
            new SubtitleEntry(new TimeSpan(0, 1, 8), new TimeSpan(0, 1, 10), "d", null, new Position(0, 20)),
            new SubtitleEntry(new TimeSpan(0, 1, 9), new TimeSpan(0, 1, 10), "d", null, new Position(0, 20)));

        var result = sut.Transform(karaoke).ToList();

        Assert.Collection(result, e => e
            .HasStart(0, 1, 0)
            .HasEnd(0, 1, 10)
            .HasContent("and"));
    }

    [Fact]
    public void Repeating_Phrases_Cleanup()
    {
        var sut = new KaraokeTransform();
        var karaoke = ToEnumerable(
            new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 20), "a"),
            new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 20), "b"),
            new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 20), "c"),
            new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 20), "."),
            new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 20), "a"),
            new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 20), "b"),
            new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 20), "c"),
            new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 20), "."));

        var result = sut.Transform(karaoke);

        Assert.Collection(result, e => e
            .HasStart(0, 1, 10)
            .HasEnd(0, 1, 20)
            .HasContent("abc."));
    }

    [Fact]
    public void Transform_By_Style()
    {
        var sut = new KaraokeTransform("karaoke,song");
        var karaoke = ToEnumerable(
            new SubtitleEntry(new TimeSpan(0, 1, 0), new TimeSpan(0, 1, 10), "a", "karaoke"),
            new SubtitleEntry(new TimeSpan(0, 1, 3), new TimeSpan(0, 1, 10), "n", "karaoke"),
            new SubtitleEntry(new TimeSpan(0, 1, 6), new TimeSpan(0, 1, 10), "d", "karaoke"),
            new SubtitleEntry(new TimeSpan(0, 1, 1), new TimeSpan(0, 1, 10), "n", "song"),
            new SubtitleEntry(new TimeSpan(0, 1, 4), new TimeSpan(0, 1, 10), "o", "song"),
            new SubtitleEntry(new TimeSpan(0, 1, 7), new TimeSpan(0, 1, 10), "w", "comment"));

        var result = sut.Transform(karaoke)
            .OrderBy(e => e.StyleName)
            .ThenBy(e => e.StartTime);

        Assert.Collection(result, e => e
                .HasStart(0, 1, 7)
                .HasEnd(0, 1, 10)
                .HasContent("w")
                .HasStyle("comment"), 
            e => e
                .HasStart(0, 1, 0)
                .HasEnd(0, 1, 10)
                .HasContent("and")
                .HasStyle("karaoke"),
            e => e
                .HasStart(0, 1, 1)
                .HasEnd(0, 1, 10)
                .HasContent("no")
                .HasStyle("song"));
    }
}