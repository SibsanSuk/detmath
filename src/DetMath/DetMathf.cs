namespace DetMath;

public static class DetMathf
{
    public static Fix64 Abs(Fix64 value)
    {
        return value.RawValue < 0 ? Fix64.FromRaw(checked(-value.RawValue)) : value;
    }

    public static Fix64 Min(Fix64 left, Fix64 right)
    {
        return left.RawValue <= right.RawValue ? left : right;
    }

    public static Fix64 Max(Fix64 left, Fix64 right)
    {
        return left.RawValue >= right.RawValue ? left : right;
    }

    public static Fix64 Clamp(Fix64 value, Fix64 min, Fix64 max)
    {
        if (min > max)
        {
            throw new System.ArgumentException("Min cannot be greater than max.");
        }

        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static Fix64 Lerp(Fix64 from, Fix64 to, Fix64 t)
    {
        return from + ((to - from) * Clamp(t, Fix64.Zero, Fix64.One));
    }

    public static Fix64 Ratio(int numerator, int denominator)
    {
        return Fix64.FromRatio(numerator, denominator);
    }
}
