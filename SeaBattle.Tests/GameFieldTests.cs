using Xunit;
using System.Collections.Generic;

public class GameFieldTests
{
    [Fact]
    public void PlaceShipTest()
    {
        var settings = new GameSettings
        {
            FieldSize = 10,
            PlayerShipLengths = new List<int> { 2, 3, 4 },
            EnemyShipLengths = new List<int> { 2, 3, 4 },
            NumberOfMines = 2,
            NumberOfBuffs = 0
        };
        var factory = new GameFieldFactory();
        var field = new GameField(settings.FieldSize, factory, settings);

        field.PlaceShip(0, 0, 3, true);

        Assert.True(field.HasShip(0, 0));
        Assert.True(field.HasShip(0, 1));
        Assert.True(field.HasShip(0, 2));
    }

    [Fact]
    public void ShootTest()
    {
        var settings = new GameSettings
        {
            FieldSize = 10,
            PlayerShipLengths = new List<int> { 2, 3, 4 },
            EnemyShipLengths = new List<int> { 2, 3, 4 },
            NumberOfMines = 2,
            NumberOfBuffs = 0
        };
        var factory = new GameFieldFactory();
        var field = new GameField(settings.FieldSize, factory, settings);

        field.PlaceShip(0, 0, 3, true);
        bool hit = field.Shoot(0, 0);

        Assert.True(hit);
    }
}