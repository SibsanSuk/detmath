using System;
using System.Globalization;

namespace DetMath;

/// <summary>
/// Fixed-point number with 2 decimal digits of precision.
/// This avoids float/double drift and keeps arithmetic deterministic.
/// Safe multiplication range: ±30,370,004.
/// </summary>
public readonly struct Fix64 : IEquatable<Fix64>, IComparable<Fix64>
{
    public const long Precision = 100L;

    public static readonly Fix64 Zero = new(0L);
    public static readonly Fix64 One = new(Precision);
    public static readonly Fix64 Half = new(Precision / 2L);

    public long RawValue { get; }

    private Fix64(long rawValue)
    {
        RawValue = rawValue;
    }

    public static Fix64 FromRaw(long rawValue)
    {
        return new Fix64(rawValue);
    }

    public static Fix64 FromInt(int value)
    {
        return new Fix64(checked(value * Precision));
    }

    public static Fix64 FromRatio(int numerator, int denominator)
    {
        if (denominator == 0)
        {
            throw new DivideByZeroException("Cannot create a fixed-point ratio with denominator 0.");
        }

        return new Fix64(checked((long)numerator * Precision / denominator));
    }

    public int CompareTo(Fix64 other)
    {
        return RawValue.CompareTo(other.RawValue);
    }

    public bool Equals(Fix64 other)
    {
        return RawValue == other.RawValue;
    }

    public override bool Equals(object? obj)
    {
        return obj is Fix64 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return RawValue.GetHashCode();
    }

    public int ToIntTruncate()
    {
        return (int)(RawValue / Precision);
    }

    public int ToIntFloor()
    {
        long q = RawValue / Precision;
        if (RawValue < 0 && RawValue % Precision != 0)
            q--;
        return checked((int)q);
    }

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

    public static Fix64 operator +(Fix64 left, Fix64 right)
    {
        return new Fix64(checked(left.RawValue + right.RawValue));
    }

    public static Fix64 operator -(Fix64 left, Fix64 right)
    {
        return new Fix64(checked(left.RawValue - right.RawValue));
    }

    public static Fix64 operator -(Fix64 value)
    {
        return new Fix64(checked(-value.RawValue));
    }

    public static Fix64 operator *(Fix64 left, Fix64 right)
    {
        return new Fix64(checked(left.RawValue * right.RawValue / Precision));
    }

    public static Fix64 operator /(Fix64 left, Fix64 right)
    {
        if (right.RawValue == 0)
        {
            throw new DivideByZeroException("Cannot divide by 0.");
        }

        return new Fix64(checked(left.RawValue * Precision / right.RawValue));
    }

    public static bool operator ==(Fix64 left, Fix64 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Fix64 left, Fix64 right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(Fix64 left, Fix64 right)
    {
        return left.RawValue < right.RawValue;
    }

    public static bool operator >(Fix64 left, Fix64 right)
    {
        return left.RawValue > right.RawValue;
    }

    public static bool operator <=(Fix64 left, Fix64 right)
    {
        return left.RawValue <= right.RawValue;
    }

    public static bool operator >=(Fix64 left, Fix64 right)
    {
        return left.RawValue >= right.RawValue;
    }
}

