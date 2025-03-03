public class Game
{
    private GameField _playerField;
    private GameField _enemyField;
    private Random _random;

    public Game()
    {
        _playerField = new GameField();
        _enemyField = new GameField();
        _random = new Random();
        PlaceShipsRandomly(_playerField, new List<int> { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 });
        PlaceShipsRandomly(_enemyField, new List<int> { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 });
        PlaceMinesAndBuffs(_playerField);
        PlaceMinesAndBuffs(_enemyField);
    }

    private void PlaceShipsRandomly(GameField field, List<int> shipLengths)
    {
        foreach (var length in shipLengths)
        {
            bool placed = false;
            while (!placed)
            {
                int x = _random.Next(0, 10);
                int y = _random.Next(0, 10);
                bool horizontal = _random.Next(2) == 0;

                if (CanPlaceShip(field, x, y, length, horizontal))
                {
                    field.PlaceShip(new Ship(x, y, length, horizontal));
                    placed = true;
                }
            }
        }
    }

    private void PlaceMinesAndBuffs(GameField field)
    {
        for (int i = 0; i < 5; i++)
        {
            int x = _random.Next(0, 10);
            int y = _random.Next(0, 10);
            if (!field.HasShip(x, y))
            {
                field.PlaceMine(x, y);
            }

            x = _random.Next(0, 10);
            y = _random.Next(0, 10);
            if (!field.HasShip(x, y))
            {
                field.PlaceBuff(x, y);
            }
        }
    }

    private bool CanPlaceShip(GameField field, int x, int y, int length, bool horizontal)
    {
        for (int i = 0; i < length; i++)
        {
            int newX = x + (horizontal ? 0 : i);
            int newY = y + (horizontal ? i : 0);

            if (newX >= 10 || newY >= 10 || field.HasShip(newX, newY))
            {
                return false;
            }
        }
        return true;
    }

    private void EnemyTurn()
    {
        int x = _random.Next(0, 10);
        int y = _random.Next(0, 10);

        if (_playerField.Shoot(x, y))
        {
            Console.WriteLine($"Enemy hit at ({x}, {y})!");
        }
        else
        {
            Console.WriteLine($"Enemy missed at ({x}, {y}).");
        }
    }

    public void Play()
    {
        while (!_playerField.AllShipsSunk() && !_enemyField.AllShipsSunk())
        {
            Console.WriteLine("Your field:");
            _playerField.DisplayField(showShips: true);
            Console.WriteLine("Enemy field:");
            _enemyField.DisplayField(showShips: false);

            Console.WriteLine("Enter coordinates to shoot (row column):");
            var input = Console.ReadLine()?.Split(' ');
            if (input != null && input.Length == 2 && int.TryParse(input[0], out int x) && int.TryParse(input[1], out int y))
            {
                if (_enemyField.Shoot(x, y))
                {
                    // Message about hit is already displayed in Shoot method
                }
                else
                {
                    Console.WriteLine("Miss!");
                }

                if (!_enemyField.AllShipsSunk())
                {
                    Console.WriteLine("Enemy's turn:");
                    EnemyTurn();
                }
            }
            else
            {
                Console.WriteLine("Invalid input format. Try again.");
            }
        }

        if (_playerField.AllShipsSunk())
        {
            Console.WriteLine("You lost! All your ships are sunk.");
        }
        else
        {
            Console.WriteLine("Congratulations! You sunk all enemy ships!");
        }
    }
}