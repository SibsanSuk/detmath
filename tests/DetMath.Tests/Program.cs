using System;

namespace DetMath.Tests;

internal static class Program
{
    private static int Main()
    {
        (string Name, Action Run)[] tests =
        {
            ("Fix64.AdditionUsesFixedPrecision", Fix64Tests.AdditionUsesFixedPrecision),
            ("Fix64.MultiplicationStaysDeterministic", Fix64Tests.MultiplicationStaysDeterministic),
            ("Fix64.DivisionRestoresExpectedValue", Fix64Tests.DivisionRestoresExpectedValue),
            ("Fix64.ClampLimitsToBounds", Fix64Tests.ClampLimitsToBounds),
            ("Fix64.ToIntFloorPositive", Fix64Tests.ToIntFloorPositive),
            ("Fix64.ToIntFloorNegativeWithRemainder", Fix64Tests.ToIntFloorNegativeWithRemainder),
            ("Fix64.ToIntFloorNegativeExact", Fix64Tests.ToIntFloorNegativeExact),
            ("Fix64.ToIntTruncateNegative", Fix64Tests.ToIntTruncateNegative),
            ("DetVector2.DotProductUsesFixedPointMath", DetVector2Tests.DotProductUsesFixedPointMath),
            ("DetVector2.ScalingAppliesToBothAxes", DetVector2Tests.ScalingAppliesToBothAxes),
            ("GridPoint.ManhattanDistanceMatchesTileTravel", GridPointTests.ManhattanDistanceMatchesTileTravel),
            ("GridPoint.ChebyshevDistanceDiagonalOnly",      GridPointTests.ChebyshevDistanceDiagonalOnly),
            ("GridPoint.ChebyshevDistanceMixedPath",         GridPointTests.ChebyshevDistanceMixedPath),
            ("GridPoint.ChebyshevDistanceIsSymmetric",       GridPointTests.ChebyshevDistanceIsSymmetric),
            ("GridPoint.ChebyshevDistanceSamePoint",         GridPointTests.ChebyshevDistanceSamePoint),
            ("DetMathf.AbsPositive",                         DetMathfTests.AbsPositive),
            ("DetMathf.AbsNegative",                         DetMathfTests.AbsNegative),
            ("DetMathf.AbsZero",                             DetMathfTests.AbsZero),
            ("DetMathf.MinReturnsSmaller",                   DetMathfTests.MinReturnsSmaller),
            ("DetMathf.MinBothEqual",                        DetMathfTests.MinBothEqual),
            ("DetMathf.MaxReturnsLarger",                    DetMathfTests.MaxReturnsLarger),
            ("DetMathf.ClampBelowMin",                       DetMathfTests.ClampBelowMin),
            ("DetMathf.ClampAboveMax",                       DetMathfTests.ClampAboveMax),
            ("DetMathf.ClampWithinRange",                    DetMathfTests.ClampWithinRange),
            ("DetMathf.LerpAtZero",                          DetMathfTests.LerpAtZero),
            ("DetMathf.LerpAtOne",                           DetMathfTests.LerpAtOne),
            ("DetMathf.LerpAtHalf",                          DetMathfTests.LerpAtHalf),
            ("DetMathf.LerpClampsAboveOne",                  DetMathfTests.LerpClampsAboveOne),
            ("Display.ProgressBar_Zero",                     DisplayTests.ProgressBar_Zero),
            ("Display.ProgressBar_Full",                     DisplayTests.ProgressBar_Full),
            ("Display.ProgressBar_Half",                     DisplayTests.ProgressBar_Half),
            ("Display.ProgressBar_ThreeQuarters",            DisplayTests.ProgressBar_ThreeQuarters),
            ("Display.ProgressBar_OverFull",                 DisplayTests.ProgressBar_OverFull),
            ("Display.ProgressBar_ClampsBeforeDisplay",      DisplayTests.ProgressBar_ClampsBeforeDisplay),
            ("Display.ProgressBar_IntegerDivisionDoesNotLoseMeaning", DisplayTests.ProgressBar_IntegerDivisionDoesNotLoseMeaning)
        };

        int passed = 0;

        foreach ((string name, Action run) in tests)
        {
            try
            {
                run();
                Console.WriteLine($"[PASS] {name}");
                passed++;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[FAIL] {name}");
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }

        Console.WriteLine($"Completed {passed}/{tests.Length} tests.");
        return 0;
    }
}

