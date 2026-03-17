namespace DetMath;

/// <summary>
/// Deterministic math functions for Fix64.
/// Mirrors Unity's <c>Mathf</c> — call <c>DetMathf.X</c> instead of <c>Mathf.X</c>.
/// </summary>
public static class DetMathf
{
    /// <summary>Returns the absolute value. Example: Abs(−3.00) → 3.00</summary>
    public static Fix64 Abs(Fix64 value)
    {
        return value.RawValue < 0 ? Fix64.FromRaw(checked(-value.RawValue)) : value;
    }

    /// <summary>Returns the lesser of two values.</summary>
    public static Fix64 Min(Fix64 left, Fix64 right)
    {
        return left.RawValue <= right.RawValue ? left : right;
    }

    /// <summary>Returns the greater of two values.</summary>
    public static Fix64 Max(Fix64 left, Fix64 right)
    {
        return left.RawValue >= right.RawValue ? left : right;
    }

    /// <summary>
    /// Clamps <paramref name="value"/> to [<paramref name="min"/>, <paramref name="max"/>].
    /// Use <c>Clamp(v, Fix64.Zero, Fix64.One)</c> for a 0–1 range.
    /// </summary>
    /// <exception cref="System.ArgumentException">Thrown when min &gt; max.</exception>
    public static Fix64 Clamp(Fix64 value, Fix64 min, Fix64 max)
    {
        if (min > max)
            throw new System.ArgumentException("Min cannot be greater than max.");

        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    /// <summary>
    /// Linearly interpolates from <paramref name="from"/> to <paramref name="to"/>.
    /// <paramref name="t"/> is clamped to [0, 1] — use for smooth transitions.
    /// Example: Lerp(2, 8, 0.50) → 5.00
    /// </summary>
    public static Fix64 Lerp(Fix64 from, Fix64 to, Fix64 t)
    {
        return from + ((to - from) * Clamp(t, Fix64.Zero, Fix64.One));
    }

    /// <summary>
    /// Shorthand for <c>Fix64.FromRatio(n, d)</c>.
    /// Example: Ratio(3, 2) → 1.50
    /// </summary>
    public static Fix64 Ratio(int numerator, int denominator)
    {
        return Fix64.FromRatio(numerator, denominator);
    }
}
