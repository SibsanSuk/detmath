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
}

