using System;

namespace DetMath;

/// <summary>
/// Immutable integer coordinate on a tile grid.
/// Use for building positions, unit positions, and path cost calculations.
/// </summary>
public readonly struct GridPoint : IEquatable<GridPoint>
{
    /// <summary>(0, 0)</summary>
    public static readonly GridPoint Zero = new(0, 0);

    /// <summary>Creates a grid coordinate at (x, y).</summary>
    public GridPoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>Horizontal tile coordinate.</summary>
    public int X { get; }

    /// <summary>Vertical tile coordinate.</summary>
    public int Y { get; }

    /// <summary>
    /// Returns the number of steps to reach <paramref name="other"/> moving in 4 directions only (up/down/left/right).
    /// Use for road networks and grid-based pathfinding.
    /// Example: (3,7) → (8,2) = 10
    /// </summary>
    public int ManhattanDistance(GridPoint other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    /// <summary>
    /// Returns the number of steps to reach <paramref name="other"/> moving in 8 directions (including diagonal).
    /// Use for unit movement where diagonal costs the same as cardinal.
    /// Example: (3,7) → (8,2) = 5
    /// </summary>
    public int ChebyshevDistance(GridPoint other)
    {
        return Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));
    }

    public bool Equals(GridPoint other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is GridPoint other && Equals(other);
    public override int GetHashCode() { unchecked { return (X * 397) ^ Y; } }
    public override string ToString() => $"({X}, {Y})";

    public static GridPoint operator +(GridPoint left, GridPoint right) => new(left.X + right.X, left.Y + right.Y);
    public static GridPoint operator -(GridPoint left, GridPoint right) => new(left.X - right.X, left.Y - right.Y);
    public static bool operator ==(GridPoint left, GridPoint right) => left.Equals(right);
    public static bool operator !=(GridPoint left, GridPoint right) => !left.Equals(right);
}
