using System;
using System.Collections.Generic;

public class GameFieldFactory
{
    private Random _random;

    public GameFieldFactory()
    {
        _random = new Random();
    }

    public Ship CreateShip(int x, int y, int length, bool horizontal)
    {
        return new Ship(x, y, length, horizontal);
    }

    public (int x, int y) CreateMine(int fieldSize)
    {
        int x = _random.Next(0, fieldSize);
        int y = _random.Next(0, fieldSize);
        return (x, y);
    }

    // Uncomment if buffs are needed
    // public (int x, int y) CreateBuff(int fieldSize)
    // {
    //     int x = _random.Next(0, fieldSize);
    //     int y = _random.Next(0, fieldSize);
    //     return (x, y);
    // }
}