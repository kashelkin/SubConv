using SubConv.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SubConv.Providers.Ass
{
    public static class AssReader
    {
        public static IEnumerable<SubtitleEntry> Read(TextReader reader)
        {
            while (reader.ReadLine() is { } line && !line.StartsWith("[Events]", StringComparison.Ordinal))
            { }

            while (reader.ReadLine() is { } line)
            {
                if (line.StartsWith("Dialogue:", StringComparison.Ordinal))
                    yield return CreateEntry(line);
            }
        }

        public static SubtitleEntry CreateEntry(string line)
        {
            var destyled = Regex.Replace(line, @"\{[^}]*\}", "");
            var splitLine = destyled.Split(',', 10);
            var beginTime = TimeSpan.ParseExact(splitLine[1], @"h\:mm\:ss\.ff", CultureInfo.InvariantCulture);
            var endTime = TimeSpan.ParseExact(splitLine[2], @"h\:mm\:ss\.ff", CultureInfo.InvariantCulture);
            var text = splitLine[9].Replace("\\N", Environment.NewLine, StringComparison.Ordinal);
            return new SubtitleEntry(beginTime, endTime, text);
        }
    }
}
