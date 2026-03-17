namespace DetMath;

/// <summary>
/// Immutable 2D vector using Fix64 components.
/// Use for directions, velocities, and influence calculations in simulation logic.
/// </summary>
public readonly struct DetVector2
{
    /// <summary>(0.00, 0.00)</summary>
    public static readonly DetVector2 Zero  = new(Fix64.Zero, Fix64.Zero);

    /// <summary>(1.00, 0.00)</summary>
    public static readonly DetVector2 UnitX = new(Fix64.One,  Fix64.Zero);

    /// <summary>(0.00, 1.00)</summary>
    public static readonly DetVector2 UnitY = new(Fix64.Zero, Fix64.One);

    /// <summary>Creates a 2D vector with the given components.</summary>
    public DetVector2(Fix64 x, Fix64 y)
    {
        X = x;
        Y = y;
    }

    /// <summary>Horizontal component.</summary>
    public Fix64 X { get; }

    /// <summary>Vertical component.</summary>
    public Fix64 Y { get; }

    /// <summary>
    /// X² + Y² — squared length of the vector.
    /// Use to compare distances without needing Sqrt.
    /// </summary>
    public Fix64 LengthSquared => (X * X) + (Y * Y);

    /// <summary>
    /// Returns the dot product of two vectors.
    /// Result is 1.00 when parallel, 0.00 when perpendicular, −1.00 when opposite.
    /// Use to measure alignment between a unit's facing direction and a target direction.
    /// </summary>
    public static Fix64 Dot(DetVector2 left, DetVector2 right)
    {
        return (left.X * right.X) + (left.Y * right.Y);
    }

    public override string ToString() => $"({X}, {Y})";

    public static DetVector2 operator +(DetVector2 left, DetVector2 right) => new(left.X + right.X, left.Y + right.Y);
    public static DetVector2 operator -(DetVector2 left, DetVector2 right) => new(left.X - right.X, left.Y - right.Y);
    public static DetVector2 operator *(DetVector2 value, Fix64 scalar)    => new(value.X * scalar, value.Y * scalar);
    public static DetVector2 operator /(DetVector2 value, Fix64 scalar)    => new(value.X / scalar, value.Y / scalar);
}
