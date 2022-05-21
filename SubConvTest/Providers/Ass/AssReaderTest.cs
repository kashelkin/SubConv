using SubConv.Providers.Ass;
using System;
using System.IO;
using Xunit;

namespace SubConvTest.Providers.Ass
{
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
                    .WithStart(0, 11, 52, 900)
                    .WithEnd(0, 11, 57, 100)
                    .WithContent("String with : should pass.")
                    .WithStyle("Colon"));
            });
        }

        [Fact]
        public void Read_Entry_With_Coma()
        {
            WithStreamReader(@"
[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
Dialogue: 0,0:11:52.90,0:11:57.10,Coma,,0000,0000,0000,,String with , should pass.",
            sr =>
            {
                var result = AssReader.Read(sr);

                Assert.Collection(result, e => e
                    .WithStart(0, 11, 52, 900)
                    .WithEnd(0, 11, 57, 100)
                    .WithContent("String with , should pass.")
                    .WithStyle("Coma"));

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
                    .WithStart(0, 0, 1, 180)
                    .WithEnd(0, 0, 6, 850)
                    .WithContent("Like an Angel with pity on nobody\nThe second line in subtitle".EnvNewLine())
                    .WithStyle("DefaultVCD"));
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
                        .WithStart(0, 11, 52, 900)
                        .WithEnd(0, 11, 57, 100)
                        .WithContent("We should ask Yang Wenli to rush\nback to Iserlohn Fortress.".EnvNewLine())
                        .WithStyle("Speech"),
                    e => e
                        .WithStart(0, 11, 57, 570)
                        .WithEnd(0, 11, 59, 930)
                        .WithContent("No, we should beg him to go back\nto Iserlohn Fortress to".EnvNewLine())
                        .WithStyle("Speech"),
                    e => e
                        .WithStart(0, 12, 00, 300)
                        .WithEnd(0, 12, 01, 970)
                        .WithContent("defeat the Empire.\nThis is too embarrassing.".EnvNewLine())
                        .WithStyle("Speech"));
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
}
