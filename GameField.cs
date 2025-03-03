public class GameField
{
    private const int Size = 10;
    private char[,] _field;
    private List<Ship> _ships;
    private List<(int x, int y)> _mines;
    private List<(int x, int y)> _buffs;

    public GameField()
    {
        _field = new char[Size, Size];
        _ships = new List<Ship>();
        _mines = new List<(int x, int y)>();
        _buffs = new List<(int x, int y)>();
        InitializeField();
    }

    private void InitializeField()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                _field[i, j] = '~';
            }
        }
    }

    public void PlaceShip(Ship ship)
    {
        _ships.Add(ship);
        foreach (var (x, y) in ship.Cells)
        {
            _field[x, y] = 'S';
        }
    }

    public void PlaceMine(int x, int y)
    {
        _mines.Add((x, y));
        _field[x, y] = 'M';
    }

    public void PlaceBuff(int x, int y)
    {
        _buffs.Add((x, y));
        _field[x, y] = 'B';
    }

    public bool Shoot(int x, int y)
    {
        if (x >= 0 && x < Size && y >= 0 && y < Size)
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
                            Console.WriteLine("Ship destroyed!");
                        }
                        else
                        {
                            Console.WriteLine("Hit!");
                        }
                    }
                }
                return true;
            }
            else if (_field[x, y] == 'M')
            {
                _field[x, y] = 'O';
                DetonateMine(x, y);
                return false;
            }
            else if (_field[x, y] == 'B')
            {
                _field[x, y] = 'O';
                ApplyBuff(x, y);
                return false;
            }
            else if (_field[x, y] == '~')
            {
                _field[x, y] = 'O';
                return false;
            }
        }
        return false;
    }

    private void DetonateMine(int x, int y)
    {
        Console.WriteLine("Mine exploded!");
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < Size && j >= 0 && j < Size)
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
                                    Console.WriteLine("Ship destroyed!");
                                }
                                else
                                {
                                    Console.WriteLine("Hit!");
                                }
                            }
                        }
                    }
                    else if (_field[i, j] == '~')
                    {
                        _field[i, j] = 'O';
                    }
                }
            }
        }
    }

    private void ApplyBuff(int x, int y)
    {
        Console.WriteLine("You found a buff!");
    }

    public void DisplayField(bool showShips = false)
    {
        Console.Write("  ");
        for (int i = 0; i < Size; i++)
        {
            Console.Write(i + " ");
        }
        Console.WriteLine();

        for (int i = 0; i < Size; i++)
        {
            Console.Write(i + " ");
            for (int j = 0; j < Size; j++)
            {
                if (showShips)
                {
                    Console.Write(_field[i, j] + " ");
                }
                else
                {
                    if (_field[i, j] == 'S' || _field[i, j] == 'M' || _field[i, j] == 'B')
                    {
                        Console.Write("~ ");
                    }
                    else
                    {
                        Console.Write(_field[i, j] + " ");
                    }
                }
            }
            Console.WriteLine();
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