using global::DetMath;

namespace DetMath.Tests;

internal static class DetVector2Tests
{
    public static void DotProductUsesFixedPointMath()
    {
        DetVector2 a = new(Fix64.FromInt(3), Fix64.FromInt(2));
        DetVector2 b = new(Fix64.FromInt(4), Fix64.FromRatio(1, 2));

        AssertEx.Equal(Fix64.FromRaw(1_300), DetVector2.Dot(a, b), "Dot product should be deterministic.");
    }

    public static void ScalingAppliesToBothAxes()
    {
        DetVector2 start = new(Fix64.FromInt(8), Fix64.FromInt(6));
        DetVector2 result = start * Fix64.FromRatio(1, 2);

        AssertEx.Equal(Fix64.FromInt(4), result.X, "Scaling should update X.");
        AssertEx.Equal(Fix64.FromInt(3), result.Y, "Scaling should update Y.");
    }
}

