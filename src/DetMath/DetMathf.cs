namespace DetMath;

public static class DetMathf
{
    public static Fix64 Clamp(Fix64 value, Fix64 min, Fix64 max)
    {
        return Fix64.Clamp(value, min, max);
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

