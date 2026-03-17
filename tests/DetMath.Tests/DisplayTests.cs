using global::DetMath;

namespace DetMath.Tests;

/// <summary>
/// Tests for converting Fix64 to display values (progress bars, labels).
/// Uses integer arithmetic only — no float involved.
/// </summary>
internal static class DisplayTests
{
    private static int ToPercent(Fix64 value)
    {
        return (int)(value.RawValue * 100L / Fix64.Precision);
    }

    public static void ProgressBar_Zero()
    {
        AssertEx.Equal(0, ToPercent(Fix64.Zero), "0.00 should display as 0%.");
    }

    public static void ProgressBar_Full()
    {
        AssertEx.Equal(100, ToPercent(Fix64.One), "1.00 should display as 100%.");
    }

    public static void ProgressBar_Half()
    {
        AssertEx.Equal(50, ToPercent(Fix64.Half), "0.50 should display as 50%.");
    }

    public static void ProgressBar_ThreeQuarters()
    {
        AssertEx.Equal(75, ToPercent(Fix64.FromRatio(3, 4)), "0.75 should display as 75%.");
    }

    public static void ProgressBar_OverFull()
    {
        // ค่าเกิน 1 (ก่อน clamp) → percent > 100 — caller ต้อง clamp เองก่อนแสดงผล
        AssertEx.Equal(130, ToPercent(Fix64.FromRatio(13, 10)), "1.30 should display as 130% before clamping.");
    }

    public static void ProgressBar_ClampsBeforeDisplay()
    {
        // pattern ที่ถูกต้อง: clamp ก่อน แล้วค่อยแปลง
        Fix64 raw     = Fix64.FromRatio(13, 10);          // 1.30 จาก simulation
        Fix64 clamped = DetMathf.Clamp(raw, Fix64.Zero, Fix64.One);  // 1.00
        AssertEx.Equal(100, ToPercent(clamped), "Clamped value should display as 100%.");
    }

    public static void ProgressBar_IntegerDivisionDoesNotLoseMeaning()
    {
        // RawValue / Precision (long ÷ long) ตัดทศนิยมทิ้ง — ต้องคูณ 100 ก่อน
        Fix64 happiness = Fix64.FromRatio(3, 4);  // 0.75, RawValue = 75
        int wrong   = (int)(happiness.RawValue / Fix64.Precision);  // 75 / 100 = 0 ❌
        int correct = ToPercent(happiness);                          // 75 * 100 / 100 = 75 ✅
        AssertEx.Equal(0,  wrong,   "Integer division without scaling loses the fractional part.");
        AssertEx.Equal(75, correct, "Multiply before divide preserves the percentage.");
    }
}
