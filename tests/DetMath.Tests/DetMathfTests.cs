using global::DetMath;

namespace DetMath.Tests;

internal static class DetMathfTests
{
    public static void AbsPositive()
    {
        AssertEx.Equal(Fix64.FromInt(3), DetMathf.Abs(Fix64.FromInt(3)), "Abs of positive should be unchanged.");
    }

    public static void AbsNegative()
    {
        AssertEx.Equal(Fix64.FromInt(3), DetMathf.Abs(Fix64.FromInt(-3)), "Abs of negative should flip sign.");
    }

    public static void AbsZero()
    {
        AssertEx.Equal(Fix64.Zero, DetMathf.Abs(Fix64.Zero), "Abs of zero should be zero.");
    }

    public static void MinReturnsSmaller()
    {
        AssertEx.Equal(Fix64.FromInt(2), DetMathf.Min(Fix64.FromInt(2), Fix64.FromInt(5)), "Min should return the smaller value.");
    }

    public static void MinBothEqual()
    {
        AssertEx.Equal(Fix64.FromInt(3), DetMathf.Min(Fix64.FromInt(3), Fix64.FromInt(3)), "Min of equal values should return that value.");
    }

    public static void MaxReturnsLarger()
    {
        AssertEx.Equal(Fix64.FromInt(5), DetMathf.Max(Fix64.FromInt(2), Fix64.FromInt(5)), "Max should return the larger value.");
    }

    public static void ClampBelowMin()
    {
        AssertEx.Equal(Fix64.Zero, DetMathf.Clamp(Fix64.FromInt(-1), Fix64.Zero, Fix64.One), "Clamp below min should return min.");
    }

    public static void ClampAboveMax()
    {
        AssertEx.Equal(Fix64.One, DetMathf.Clamp(Fix64.FromInt(2), Fix64.Zero, Fix64.One), "Clamp above max should return max.");
    }

    public static void ClampWithinRange()
    {
        AssertEx.Equal(Fix64.Half, DetMathf.Clamp(Fix64.Half, Fix64.Zero, Fix64.One), "Clamp within range should return value unchanged.");
    }

    public static void LerpAtZero()
    {
        AssertEx.Equal(Fix64.FromInt(2), DetMathf.Lerp(Fix64.FromInt(2), Fix64.FromInt(8), Fix64.Zero), "Lerp at t=0 should return from.");
    }

    public static void LerpAtOne()
    {
        AssertEx.Equal(Fix64.FromInt(8), DetMathf.Lerp(Fix64.FromInt(2), Fix64.FromInt(8), Fix64.One), "Lerp at t=1 should return to.");
    }

    public static void LerpAtHalf()
    {
        // from=2, to=8, t=0.50 → 2 + (8-2)*0.50 = 5.00
        AssertEx.Equal(Fix64.FromInt(5), DetMathf.Lerp(Fix64.FromInt(2), Fix64.FromInt(8), Fix64.Half), "Lerp at t=0.50 should return midpoint.");
    }

    public static void LerpClampsAboveOne()
    {
        // t=1.50 should be clamped to 1 → result = to
        AssertEx.Equal(Fix64.FromInt(8), DetMathf.Lerp(Fix64.FromInt(2), Fix64.FromInt(8), Fix64.FromRatio(3, 2)), "Lerp with t>1 should clamp to 'to'.");
    }
}
