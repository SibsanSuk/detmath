using global::DetMath;

namespace DetMath.Tests;

internal static class GridPointTests
{
    public static void ManhattanDistanceMatchesTileTravel()
    {
        GridPoint market = new(2, 3);
        GridPoint house = new(8, 6);

        AssertEx.Equal(9, market.ManhattanDistance(house), "Grid distance should match city-builder tile movement.");
    }

    public static void ChebyshevDistanceDiagonalOnly()
    {
        // (3,7) → (8,2): dx=5, dy=5 → max(5,5) = 5
        GridPoint a = new(3, 7);
        GridPoint b = new(8, 2);

        AssertEx.Equal(5, a.ChebyshevDistance(b), "Pure diagonal path should cost max(dx,dy).");
    }

    public static void ChebyshevDistanceMixedPath()
    {
        // (3,7) → (8,3): dx=5, dy=4 → max(5,4) = 5
        GridPoint a = new(3, 7);
        GridPoint b = new(8, 3);

        AssertEx.Equal(5, a.ChebyshevDistance(b), "Mixed diagonal+cardinal path should cost max(dx,dy).");
    }

    public static void ChebyshevDistanceIsSymmetric()
    {
        GridPoint a = new(1, 4);
        GridPoint b = new(6, 2);

        AssertEx.Equal(a.ChebyshevDistance(b), b.ChebyshevDistance(a), "Chebyshev distance must be symmetric.");
    }

    public static void ChebyshevDistanceSamePoint()
    {
        GridPoint a = new(5, 5);

        AssertEx.Equal(0, a.ChebyshevDistance(a), "Distance to self must be 0.");
    }
}

