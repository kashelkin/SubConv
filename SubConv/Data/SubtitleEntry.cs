namespace SubConv.Data
{
    public class SubtitleEntry
    {
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public string Content { get; }

        public SubtitleEntry(TimeSpan startTime, TimeSpan endTime, string content)
        {
            StartTime = startTime;
            EndTime = endTime;
            Content = content;
        }
    }
}
