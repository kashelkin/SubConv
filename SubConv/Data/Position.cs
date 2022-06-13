using System;

namespace SubConv.Data
{
    public class Position : IEquatable<Position>
    {
        public decimal X { get; }
        public decimal Y { get; }

        public Position(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Position? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Position? left, Position? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Position? left, Position? right)
        {
            return !Equals(left, right);
        }
    }
}
