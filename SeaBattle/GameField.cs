// filepath: /d:/Student/UNI/itproject/lab/lab_1/SeaBattle/GameField.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class GameField
{
    private int _size;
    private char[,] _field;
    private List<Ship> _ships;
    private List<(int x, int y)> _mines;
    private GameFieldFactory _factory;
    private GameSettings _settings;

    public GameField(int size, GameFieldFactory factory, GameSettings settings)
    {
        _size = size;
        _field = new char[_size, _size];
        _ships = new List<Ship>();
        _mines = new List<(int x, int y)>();
        _factory = factory;
        _settings = settings;
        InitializeField();
    }

    private void InitializeField()
    {
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                _field[i, j] = '~';
            }
        }
    }

    public void PlaceShip(int x, int y, int length, bool horizontal)
    {
        var ship = _factory.CreateShip(x, y, length, horizontal);
        _ships.Add(ship);
        foreach (var (sx, sy) in ship.Cells)
        {
            _field[sx, sy] = 'S';
        }
    }

    public void PlaceMine()
    {
        var (x, y) = _factory.CreateMine(_size);
        _mines.Add((x, y));
        _field[x, y] = 'M';
    }

    public bool Shoot(int x, int y)
    {
        if (x >= 0 && x < _size && y >= 0 && y < _size)
        {
            if (_field[x, y] == 'S')
            {
                _field[x, y] = 'X';
                foreach (var ship in _ships)
                {
                    if (ship.Hit(x, y))
                    {
                        if (ship.IsSunk())
                        {
                            Console.WriteLine("Ship destroyed at (" + x + ", " + y + ")!");
                        }
                        else
                        {
                            Console.WriteLine("Hit at (" + x + ", " + y + ")!");
                        }
                    }
                }
                return true;
            }
            else if (_field[x, y] == 'M')
            {
                _field[x, y] = 'E';
                DetonateMine(x, y);
                return false;
            }
            else if (_field[x, y] == '~')
            {
                _field[x, y] = 'O';
                Console.WriteLine("Miss at (" + x + ", " + y + ")!");
                return false;
            }
        }
        return false;
    }

    private void DetonateMine(int x, int y)
    {
        Console.WriteLine("Mine exploded at (" + x + ", " + y + ")!");
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < _size && j >= 0 && j < _size)
                {
                    if (_field[i, j] == 'S')
                    {
                        _field[i, j] = 'X';
                        foreach (var ship in _ships)
                        {
                            if (ship.Hit(i, j))
                            {
                                if (ship.IsSunk())
                                {
                                    Console.WriteLine("Ship destroyed at (" + i + ", " + j + ")!");
                                }
                                else
                                {
                                    Console.WriteLine("Hit at (" + i + ", " + j + ")!");
                                }
                            }
                        }
                    }
                    else if (_field[i, j] == '~')
                    {
                        _field[i, j] = 'E';
                    }
                }
            }
        }
    }

    public void DisplayField(bool showShips = false)
    {
        Console.Write("  ");
        for (int i = 0; i < _size; i++)
        {
            Console.Write(i + " ");
        }
        Console.WriteLine();

        for (int i = 0; i < _size; i++)
        {
            Console.Write(i + " ");
            for (int j = 0; j < _size; j++)
            {
                if (showShips)
                {
                    SetConsoleColor(_field[i, j]);
                    Console.Write(_field[i, j] + " ");
                    Console.ResetColor();
                }
                else
                {
                    if (_field[i, j] == 'S' || _field[i, j] == 'M')
                    {
                        SetConsoleColor('~');
                        Console.Write("~ ");
                        Console.ResetColor();
                    }
                    else
                    {
                        SetConsoleColor(_field[i, j]);
                        Console.Write(_field[i, j] + " ");
                        Console.ResetColor();
                    }
                }
            }
            Console.WriteLine();
        }
    }

    private void SetConsoleColor(char symbol)
    {
        switch (symbol)
        {
            case 'S':
                Console.ForegroundColor = GameSettings.HexToConsoleColor(_settings.ShipColor);
                break;
            case 'M':
                Console.ForegroundColor = GameSettings.HexToConsoleColor(_settings.MineColor);
                break;
            case 'X':
                Console.ForegroundColor = GameSettings.HexToConsoleColor(_settings.HitColor);
                break;
            case 'O':
                Console.ForegroundColor = GameSettings.HexToConsoleColor(_settings.MissColor);
                break;
            case 'E':
                Console.ForegroundColor = GameSettings.HexToConsoleColor(_settings.ExplosionColor);
                break;
            case '~':
                Console.ForegroundColor = GameSettings.HexToConsoleColor(_settings.FieldColor);
                break;
            default:
                Console.ForegroundColor = GameSettings.HexToConsoleColor(_settings.DefaultColor);
                break;
        }
    }

    public bool HasShip(int x, int y)
    {
        return _field[x, y] == 'S';
    }

    public bool AllShipsSunk()
    {
        return _ships.All(ship => ship.IsSunk());
    }
}