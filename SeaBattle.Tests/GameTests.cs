using Xunit;
using System.IO;

public class GameTests
{
    [Fact]
    public void TestGameInitialization()
    {
        var settings = GameSettings.Load("appsettings.test.json");
        var factory = new GameFieldFactory();
        var game = new Game(settings, factory);

        Assert.NotNull(game);
    }

    // [Fact]
    // public void TestEnemyTurn()
    // {
    //     var settings = GameSettings.Load("appsettings.test.json");
    //     var factory = new GameFieldFactory();
    //     var game = new Game(settings, factory);

    //     game.Play();
        
    //     Assert.NotNull(game.PlayerField);
    //     Assert.NotNull(game.EnemyField);
    // }
}