using global::DetMath;

namespace DetMath.Tests;

internal static class Fix64Tests
{
    public static void AdditionUsesFixedPrecision()
    {
        Fix64 income = Fix64.FromRatio(3, 2);
        Fix64 upkeep = Fix64.FromRatio(1, 4);

        AssertEx.Equal(Fix64.FromRaw(175), income + upkeep, "Fixed-point addition should preserve raw precision.");
    }

    public static void MultiplicationStaysDeterministic()
    {
        Fix64 taxRate = Fix64.FromRatio(15, 100);
        Fix64 population = Fix64.FromInt(240);

        AssertEx.Equal(Fix64.FromRaw(3_600), population * taxRate, "Multiplication should stay in fixed-point space.");
    }

    public static void DivisionRestoresExpectedValue()
    {
        Fix64 stored = Fix64.FromInt(12);
        Fix64 workers = Fix64.FromInt(4);

        AssertEx.Equal(Fix64.FromInt(3), stored / workers, "Division should return the expected quotient.");
    }

    public static void ClampLimitsToBounds()
    {
        Fix64 result = DetMathf.Clamp(Fix64.FromInt(9), Fix64.Zero, Fix64.FromInt(5));

        AssertEx.Equal(Fix64.FromInt(5), result, "Clamp should limit values above max.");
    }

    public static void ToIntFloorPositive()
    {
        Fix64 value = Fix64.FromRatio(7, 2); // 3.50

        AssertEx.Equal(3, value.ToIntFloor(), "Floor of 3.50 should be 3.");
    }

    public static void ToIntFloorNegativeWithRemainder()
    {
        Fix64 value = Fix64.FromRaw(-150); // -1.50

        AssertEx.Equal(-2, value.ToIntFloor(), "Floor of -1.50 should be -2, not -1.");
    }

    public static void ToIntFloorNegativeExact()
    {
        Fix64 value = Fix64.FromInt(-2); // -2.00

        AssertEx.Equal(-2, value.ToIntFloor(), "Floor of exact -2.00 should be -2.");
    }

    public static void ToIntTruncateNegative()
    {
        Fix64 value = Fix64.FromRaw(-150); // -1.50

        AssertEx.Equal(-1, value.ToIntTruncate(), "Truncate of -1.50 toward zero should be -1.");
    }
}

