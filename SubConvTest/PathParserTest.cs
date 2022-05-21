using SubConv;
using Xunit;

namespace SubConvTest
{
    public class PathParserTest
    {
        [Theory]
        [InlineData("subtitles.ass", null, "srt", "subtitles.srt")]
        [InlineData("subtitles.ass", "", "srt", "subtitles.srt")]
        [InlineData("subtitles.ass", null, ".srt", "subtitles.srt")]
        [InlineData(@"d:\subtitles\episode 01.ass", null, "srt", @"d:\subtitles\episode 01.srt")]
        [InlineData(@"d:\subtitles\episode 01.ass", @"d:\subtitles\converted", "srt", @"d:\subtitles\converted\episode 01.srt")]
        public void Test(string inputFile, string? outputPath, string extension, string expected)
        {
            var result = PathParser.GenerateOutputFileName(inputFile.FixSlash(), outputPath.FixSlash(), extension);

            Assert.Equal(expected.FixSlash(), result);
        }
    }
}
