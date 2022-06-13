using SubConv.Providers.Ass;
using System;
using System.IO;
using Xunit;

namespace SubConvTest.Providers.Ass;

public class AssReaderTest
{
    [Fact]
    public void Read_Entry_With_Colon()
    {
        WithStreamReader(@"
[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
Dialogue: 0,0:11:52.90,0:11:57.10,Colon,,0000,0000,0000,,String with : should pass.",
            sr =>
            {
                var result = AssReader.Read(sr);

                Assert.Collection(result, e => e
                    .HasStart(0, 11, 52, 900)
                    .HasEnd(0, 11, 57, 100)
                    .HasContent("String with : should pass.")
                    .HasStyle("Colon"));
            });
    }

    [Fact]
    public void Read_Entry_With_Comma()
    {
        WithStreamReader(@"
[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
Dialogue: 0,0:11:52.90,0:11:57.10,Comma,,0000,0000,0000,,String with , should pass.",
            sr =>
            {
                var result = AssReader.Read(sr);

                Assert.Collection(result, e => e
                    .HasStart(0, 11, 52, 900)
                    .HasEnd(0, 11, 57, 100)
                    .HasContent("String with , should pass.")
                    .HasStyle("Comma"));

            });
    }

    [Fact]
    public void Ass_Style_Removed()
    {
        WithStreamReader(@"
[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
Dialogue: 0,0:00:01.18,0:00:06.85,DefaultVCD, NTP,0000,0000,0000,,{\pos(400,570)}Like an Angel with pity on nobody\NThe second line in subtitle",
            sr =>
            {
                var result = AssReader.Read(sr);

                Assert.Collection(result, e => e
                    .HasStart(0, 0, 1, 180)
                    .HasEnd(0, 0, 6, 850)
                    .HasContent("Like an Angel with pity on nobody\nThe second line in subtitle".EnvNewLine())
                    .HasStyle("DefaultVCD"));
            });
    }

    [Fact]
    public void Read_Multiple_Entries()
    {
        WithStreamReader(@"
[Script Info]
;Test script

[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
Dialogue: 0,0:11:52.90,0:11:57.10,Speech,,0000,0000,0000,,We should ask Yang Wenli to rush\Nback to Iserlohn Fortress.
Dialogue: 0,0:11:57.57,0:11:59.93,Speech,,0000,0000,0000,,No, we should beg him to go back\Nto Iserlohn Fortress to
Dialogue: 0,0:12:00.30,0:12:01.97,Speech,,0000,0000,0000,,defeat the Empire.\NThis is too embarrassing.",
            sr =>
            {
                var result = AssReader.Read(sr);

                Assert.Collection(result, e => e
                        .HasStart(0, 11, 52, 900)
                        .HasEnd(0, 11, 57, 100)
                        .HasContent("We should ask Yang Wenli to rush\nback to Iserlohn Fortress.".EnvNewLine())
                        .HasStyle("Speech"),
                    e => e
                        .HasStart(0, 11, 57, 570)
                        .HasEnd(0, 11, 59, 930)
                        .HasContent("No, we should beg him to go back\nto Iserlohn Fortress to".EnvNewLine())
                        .HasStyle("Speech"),
                    e => e
                        .HasStart(0, 12, 00, 300)
                        .HasEnd(0, 12, 01, 970)
                        .HasContent("defeat the Empire.\nThis is too embarrassing.".EnvNewLine())
                        .HasStyle("Speech"));
            });
    }

    [Fact]
    public void Reads_Position()
    {
        WithStreamReader(@"
[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
Dialogue: 2,0:04:49.79,0:04:57.65,Jibun Wo,,0,0,0,fx,{\fad(0,0)\shad0\bord3\an5\pos(322.5,57)\fs0\frz360\blur15\1c&H00008F&\alpha&H0&\t(0,500,\fs0\blur0\fr0\1c&HFFFFFF&)\t(400,500,\alpha&H0&)\3c&H31358D&}Text",
            sr =>
            {
                var result = AssReader.Read(sr);

                Assert.Collection(result, e => e
                    .HasStart(0, 4, 49, 790)
                    .HasEnd(0, 4, 57, 650)
                    .HasContent("Text")
                    .HasPosition(322.5M, 57)
                );
            });
    }

    private static void WithStreamReader(string script, Action<StreamReader> action)
    {
        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        streamWriter.WriteLine(script);
        streamWriter.Flush();
        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);

        action(streamReader);
    }
}