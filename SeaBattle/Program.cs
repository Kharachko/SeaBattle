using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Sea Battle");
        var settings = GameSettings.Load("appsettings.json");
        var factory = new GameFieldFactory();
        Game game = new Game(settings, factory);
        game.Play();
    }
}