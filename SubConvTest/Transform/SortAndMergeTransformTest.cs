﻿using SubConv.Data;
using SubConv.Transform;
using System;
using Xunit;

namespace SubConvTest.Transform;

public class SortAndMergeTransformTest : BaseTransformTest
{
    [Fact]
    public void Merge_Single_Entry()
    {
        var entry = new SubtitleEntry(TimeSpan.MinValue, TimeSpan.MaxValue, "Content");

        var sut = new SortAndMergeTransform();
        var result = sut.Transform(ToEnumerable(entry));

        Assert.Collection(result, e => e
            .HasStart(TimeSpan.MinValue)
            .HasEnd(TimeSpan.MaxValue)
            .HasContent("Content")
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

        var sut = new SortAndMergeTransform();
        var result = sut.Transform(ToEnumerable(entry1, entry2));

        Assert.Collection(result, e => e
                .HasStart(0, 1, 0)
                .HasEnd(0, 4, 0)
                .HasContent("Entry1"),
            e => e
                .HasStart(0, 4, 0)
                .HasEnd(0, 5, 0)
                .HasContent("Entry1\nEntry2".EnvNewLine()),
            e => e
                .HasStart(0, 5, 0)
                .HasEnd(0, 7, 0)
                .HasContent("Entry2"));
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

        var sut = new SortAndMergeTransform();
        var result = sut.Transform(ToEnumerable(entry1, entry2));

        Assert.Collection(result, e => e
                .HasStart(0, 1, 0)
                .HasEnd(0, 3, 0)
                .HasContent("Entry1"),
            e => e
                .HasStart(0, 4, 0)
                .HasEnd(0, 7, 0)
                .HasContent("Entry2"));
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

        var sut = new SortAndMergeTransform();
        var result = sut.Transform(ToEnumerable(entry1, entry2));

        Assert.Collection(result, e => e
                .HasStart(0, 1, 0)
                .HasEnd(0, 4, 0)
                .HasContent("Entry1"),
            e => e
                .HasStart(0, 4, 0)
                .HasEnd(0, 7, 0)
                .HasContent("Entry2"));
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

        var sut = new SortAndMergeTransform();
        var result = sut.Transform(ToEnumerable(entry1, entry2));

        Assert.Collection(result, e => e
            .HasStart(0, 1, 0)
            .HasEnd(0, 4, 0)
            .HasContent("Entry1\nEntry2".EnvNewLine()));
    }

    [Fact]
    public void Default_Comparer_Sorts_By_StartTime()
    {
        var entry1 = new SubtitleEntry(
            new TimeSpan(0, 1, 0), 
            new TimeSpan(0, 5, 0),
            "Entry1");
        var entry2 = new SubtitleEntry(
            new TimeSpan(0, 4, 0),
            new TimeSpan(0, 7, 0),
            "Entry2");

        var sut = new SortAndMergeTransform();
        var result = sut.Transform(ToEnumerable(entry2, entry1));

        Assert.Collection(result, e => e
                .HasStart(0, 1, 0)
                .HasEnd(0, 4, 0)
                .HasContent("Entry1"),
            e => e
                .HasStart(0, 4, 0)
                .HasEnd(0, 5, 0)
                .HasContent("Entry1\nEntry2".EnvNewLine()),
            e => e
                .HasStart(0, 5, 0)
                .HasEnd(0, 7, 0)
                .HasContent("Entry2"));
    }

    [Fact]
    public void Custom_Comparer_Applies()
    {
        var entry1 = new SubtitleEntry(
            new TimeSpan(0, 1, 0), 
            new TimeSpan(0, 7, 0),
            "[Title Entry]",
            "Title");
        var entry2 = new SubtitleEntry(
            new TimeSpan(0, 4, 0),
            new TimeSpan(0, 7, 0),
            "Entry2",
            "Default");

        var sut = new SortAndMergeTransform(new StyleSubtitleComparer(new[] { "*", "Title" }));
        var result = sut.Transform(ToEnumerable(entry1, entry2));

        Assert.Collection(result, e => e
                .HasStart(0, 1, 0)
                .HasEnd(0, 4, 0)
                .HasContent("[Title Entry]"),
            e => e
                .HasStart(0, 4, 0)
                .HasEnd(0, 7, 0)
                .HasContent("Entry2\n[Title Entry]".EnvNewLine()));
    }

    [Fact]
    public void Zero_Entries()
    {
        var result = new SortAndMergeTransform().Transform(Array.Empty<SubtitleEntry>());

        Assert.Empty(result);
    }
}