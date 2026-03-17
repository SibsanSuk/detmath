using System;

namespace DetMath;

public readonly struct GridPoint : IEquatable<GridPoint>
{
    public static readonly GridPoint Zero = new(0, 0);

    public GridPoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }

    public int Y { get; }

    public int ManhattanDistance(GridPoint other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    public int ChebyshevDistance(GridPoint other)
    {
        return Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));
    }

    public bool Equals(GridPoint other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is GridPoint other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (X * 397) ^ Y;
        }
    }

    public static GridPoint operator +(GridPoint left, GridPoint right)
    {
        return new GridPoint(left.X + right.X, left.Y + right.Y);
    }

    public static GridPoint operator -(GridPoint left, GridPoint right)
    {
        return new GridPoint(left.X - right.X, left.Y - right.Y);
    }

    public static bool operator ==(GridPoint left, GridPoint right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GridPoint left, GridPoint right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
