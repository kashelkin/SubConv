namespace SubConv.Data
{
    public class SubtitleEntry
    {
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public string Content { get; }
        public string? StyleName { get; }
        public Position? Position { get; }

        public SubtitleEntry(TimeSpan startTime, TimeSpan endTime, string content)
        {
            StartTime = startTime;
            EndTime = endTime;
            Content = content;
        }

        public SubtitleEntry(TimeSpan startTime, TimeSpan endTime, string content, string? styleName)
        {
            StartTime = startTime;
            EndTime = endTime;
            Content = content;
            StyleName = styleName;
        }

        public SubtitleEntry(TimeSpan startTime, TimeSpan endTime, string content, string? styleName, Position? position)
        {
            StartTime = startTime;
            EndTime = endTime;
            Content = content;
            StyleName = styleName;
            Position = position;
        }

        public SubtitleEntry WithContent(string content) => new(StartTime, EndTime, content, StyleName, Position);
    }
}
