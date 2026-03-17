using System;
using System.Globalization;

namespace DetMath;

/// <summary>
/// Deterministic fixed-point number with 2 decimal places.
/// All arithmetic uses <c>long</c> integers — no float, no double.
/// Identical results on every platform (x86, ARM, etc.).
/// Safe multiplication range: ±30,370,004.
/// </summary>
public readonly struct Fix64 : IEquatable<Fix64>, IComparable<Fix64>
{
    /// <summary>Internal scale factor. 1 unit = 100 raw.</summary>
    public const long Precision = 100L;

    /// <summary>0.00</summary>
    public static readonly Fix64 Zero = new(0L);

    /// <summary>1.00</summary>
    public static readonly Fix64 One = new(Precision);

    /// <summary>0.50</summary>
    public static readonly Fix64 Half = new(Precision / 2L);

    /// <summary>
    /// Internal integer representation. Divide by <see cref="Precision"/> to get the real value.
    /// Use for display only: <c>(float)value.RawValue / Fix64.Precision</c>
    /// </summary>
    public long RawValue { get; }

    private Fix64(long rawValue)
    {
        RawValue = rawValue;
    }

    /// <summary>
    /// Creates a Fix64 directly from a raw internal value.
    /// Only use when you know the raw value explicitly.
    /// </summary>
    public static Fix64 FromRaw(long rawValue)
    {
        return new Fix64(rawValue);
    }

    /// <summary>
    /// Creates a Fix64 from a whole integer. Example: <c>FromInt(3)</c> → 3.00
    /// </summary>
    public static Fix64 FromInt(int value)
    {
        return new Fix64(checked(value * Precision));
    }

    /// <summary>
    /// Creates a Fix64 from a ratio. Example: <c>FromRatio(3, 2)</c> → 1.50
    /// Prefer this over float literals. Truncates toward zero if not exact.
    /// </summary>
    /// <exception cref="DivideByZeroException">Thrown when denominator is 0.</exception>
    public static Fix64 FromRatio(int numerator, int denominator)
    {
        if (denominator == 0)
        {
            throw new DivideByZeroException("Cannot create a fixed-point ratio with denominator 0.");
        }

        return new Fix64(checked((long)numerator * Precision / denominator));
    }

    /// <summary>
    /// Converts to the largest integer less than or equal to this value (floor toward −∞).
    /// Example: −1.50 → −2. Use for completed cycle counts.
    /// </summary>
    public int ToIntFloor()
    {
        long q = RawValue / Precision;
        if (RawValue < 0 && RawValue % Precision != 0)
            q--;
        return checked((int)q);
    }

    /// <summary>
    /// Converts to integer by discarding the fractional part (truncate toward zero).
    /// Example: −1.50 → −1.
    /// </summary>
    public int ToIntTruncate()
    {
        return (int)(RawValue / Precision);
    }

    /// <summary>
    /// Returns the value formatted as a decimal string. Example: "1.50"
    /// Use for display and logging only — do not parse back into Fix64.
    /// </summary>
    public override string ToString()
    {
        bool isNegative = RawValue < 0;
        ulong absolute = isNegative ? (ulong)(-(RawValue + 1L)) + 1UL : (ulong)RawValue;
        ulong whole = absolute / (ulong)Precision;
        ulong fraction = absolute % (ulong)Precision;

        return string.Format(
            CultureInfo.InvariantCulture,
            "{0}{1}.{2:00}",
            isNegative ? "-" : string.Empty,
            whole,
            fraction);
    }

    public int CompareTo(Fix64 other) => RawValue.CompareTo(other.RawValue);
    public bool Equals(Fix64 other) => RawValue == other.RawValue;
    public override bool Equals(object? obj) => obj is Fix64 other && Equals(other);
    public override int GetHashCode() => RawValue.GetHashCode();

    public static Fix64 operator +(Fix64 left, Fix64 right) => new(checked(left.RawValue + right.RawValue));
    public static Fix64 operator -(Fix64 left, Fix64 right) => new(checked(left.RawValue - right.RawValue));
    public static Fix64 operator -(Fix64 value)             => new(checked(-value.RawValue));
    public static Fix64 operator *(Fix64 left, Fix64 right) => new(checked(left.RawValue * right.RawValue / Precision));
    public static Fix64 operator /(Fix64 left, Fix64 right)
    {
        if (right.RawValue == 0)
            throw new DivideByZeroException("Cannot divide by 0.");
        return new Fix64(checked(left.RawValue * Precision / right.RawValue));
    }

    public static bool operator ==(Fix64 left, Fix64 right)  => left.Equals(right);
    public static bool operator !=(Fix64 left, Fix64 right)  => !left.Equals(right);
    public static bool operator < (Fix64 left, Fix64 right)  => left.RawValue < right.RawValue;
    public static bool operator > (Fix64 left, Fix64 right)  => left.RawValue > right.RawValue;
    public static bool operator <=(Fix64 left, Fix64 right)  => left.RawValue <= right.RawValue;
    public static bool operator >=(Fix64 left, Fix64 right)  => left.RawValue >= right.RawValue;
}
