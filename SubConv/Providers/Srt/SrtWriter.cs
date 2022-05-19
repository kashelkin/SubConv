using SubConv.Data;
using SubConv.Processing;
using System.Globalization;
using System.Text;

namespace SubConv.Providers.Srt
{
    public static class SrtWriter
    {
        public static void Write(TextWriter writer, IEnumerable<SubtitleEntry> entries)
        {
            foreach (var entry in SubtitleMerge.Merge(entries).Select((e, i) => new {Entry = e, Number = i + 1}))
            {
                if (entry.Number > 1)
                    writer.WriteLine();

                writer.WriteLine(SerializeEntry(entry.Entry, entry.Number));
            }
        }

        private static string SerializeEntry(SubtitleEntry entry, int i)
        {
            var sTime = entry.StartTime.ToString(@"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);
            var eTime = entry.EndTime.ToString(@"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);
            var sb = new StringBuilder()
                .AppendLine(i.ToString(CultureInfo.InvariantCulture))
                .AppendLine(CultureInfo.InvariantCulture, $"{sTime} --> {eTime}")
                .AppendLine(entry.Content);

                return sb.ToString();
        }
    }
}
