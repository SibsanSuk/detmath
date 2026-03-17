namespace DetMath;

public readonly struct DetVector2
{
    public static readonly DetVector2 Zero = new(Fix64.Zero, Fix64.Zero);
    public static readonly DetVector2 UnitX = new(Fix64.One, Fix64.Zero);
    public static readonly DetVector2 UnitY = new(Fix64.Zero, Fix64.One);

    public DetVector2(Fix64 x, Fix64 y)
    {
        X = x;
        Y = y;
    }

    public Fix64 X { get; }

    public Fix64 Y { get; }

    public Fix64 LengthSquared => (X * X) + (Y * Y);

    public static DetVector2 operator +(DetVector2 left, DetVector2 right)
    {
        return new DetVector2(left.X + right.X, left.Y + right.Y);
    }

    public static DetVector2 operator -(DetVector2 left, DetVector2 right)
    {
        return new DetVector2(left.X - right.X, left.Y - right.Y);
    }

    public static DetVector2 operator *(DetVector2 value, Fix64 scalar)
    {
        return new DetVector2(value.X * scalar, value.Y * scalar);
    }

    public static DetVector2 operator /(DetVector2 value, Fix64 scalar)
    {
        return new DetVector2(value.X / scalar, value.Y / scalar);
    }

    public static Fix64 Dot(DetVector2 left, DetVector2 right)
    {
        return (left.X * right.X) + (left.Y * right.Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

