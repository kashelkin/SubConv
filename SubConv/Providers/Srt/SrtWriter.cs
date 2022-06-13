using SubConv.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SubConv.Providers.Srt;

public static class SrtWriter
{
    public static void Write(TextWriter writer, IEnumerable<SubtitleEntry> entries)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        foreach (var entry in entries.OrderBy(e => e.StartTime).Select((e, i) => new {Entry = e, Number = i + 1}))
        {
            if (entry.Number > 1)
            {
                writer.WriteLine();
                writer.WriteLine();
            }

            writer.Write(SerializeEntry(entry.Entry, entry.Number));
        }
    }

    private static string SerializeEntry(SubtitleEntry entry, int i)
    {
        var sb = new StringBuilder()
            .AppendLine(i.ToString(CultureInfo.InvariantCulture))
            .AppendLine(CultureInfo.InvariantCulture, $"{RenderTime(entry.StartTime)} --> {RenderTime(entry.EndTime)}")
            .Append(RemoveEmptyLines(entry.Content));

        return sb.ToString();
    }

    private static string RenderTime(TimeSpan time) =>
        time.ToString(@"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);

    private static string RemoveEmptyLines(string value) =>
        Regex.Replace(value,
            "(" + Regex.Escape(Environment.NewLine) + ")" + "{2,}",
            Environment.NewLine);
}