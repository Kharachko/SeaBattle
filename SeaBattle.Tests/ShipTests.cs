using Xunit;
public class ShipTests
{
    [Fact]
    public void ShipInitializationTest()
    {
        var ship = new Ship(0, 0, 3, true);

        Assert.Equal(3, ship.Cells.Count);
        Assert.Contains((0, 0), ship.Cells);
        Assert.Contains((0, 1), ship.Cells);
        Assert.Contains((0, 2), ship.Cells);
    }

    [Fact]
    public void ShipHitTest()
    {
        var ship = new Ship(0, 0, 2, true);

        bool hit = ship.Hit(0, 0);

        Assert.True(hit);
        Assert.False(ship.IsSunk());
    }

    [Fact]
    public void ShipSunkTest()
    {
        var ship = new Ship(0, 0, 1, true);

        ship.Hit(0, 0);

        Assert.True(ship.IsSunk());
    }
}