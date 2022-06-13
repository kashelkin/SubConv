using System;

namespace SubConv.Transform.Internal;

internal class TimeInterval
{
    public TimeSpan Start { get; }
    public TimeSpan End { get; }

    public TimeInterval(TimeSpan start, TimeSpan end)
    {
        Start = start; 
        End = end; 
    }

    public bool Intersects(TimeSpan start, TimeSpan end) =>
        this.Start < end 
        && this.End > start;
}