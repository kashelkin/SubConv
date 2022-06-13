using SubConv.Data;
using SubConv.Providers.Srt;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SubConvTest.Providers.Srt;

public class SrtWriterTest
{
    [Fact]
    public void Entry_Has_No_Empty_Lines()
    {
        var entry = new SubtitleEntry(
            new TimeSpan(2, 10, 12),
            new TimeSpan(2, 10, 15),
            "Line1\n\nLine2\n\n\nLine3\nLine4".EnvNewLine(),
            "Default");

        var result = ReadStream(sw => SrtWriter.Write(sw, ToEnumerable(entry)));

        Assert.Equal(
            @"1
02:10:12,000 --> 02:10:15,000
Line1
Line2
Line3
Line4", result);
    }

    private static string ReadStream(Action<StreamWriter> action)
    {
        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);

        action(streamWriter);

        streamWriter.Flush();
        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        return streamReader.ReadToEnd();
    }

    private static IEnumerable<T> ToEnumerable<T>(params T[] data) => data;
}