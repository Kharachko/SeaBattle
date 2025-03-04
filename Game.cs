using System;
using System.Collections.Generic;

public class Game
{
    private GameField _playerField;
    private GameField _enemyField;
    private Random _random;
    private GameSettings _settings;
    private GameFieldFactory _factory;

    public Game()
    {
        _settings = GameSettings.Load("appsettings.json");
        _factory = new GameFieldFactory();
        _playerField = new GameField(_settings.FieldSize, _factory);
        _enemyField = new GameField(_settings.FieldSize, _factory);
        _random = new Random();
        PlaceShipsRandomly(_playerField, _settings.PlayerShipLengths);
        PlaceShipsRandomly(_enemyField, _settings.PlayerShipLengths);
        PlaceMines(_playerField, _settings.NumberOfMines);
        PlaceMines(_enemyField, _settings.NumberOfMines);
    }

    private void PlaceShipsRandomly(GameField field, List<int> shipLengths)
    {
        foreach (var length in shipLengths)
        {
            bool placed = false;
            while (!placed)
            {
                int x = _random.Next(0, _settings.FieldSize);
                int y = _random.Next(0, _settings.FieldSize);
                bool horizontal = _random.Next(2) == 0;

                if (CanPlaceShip(field, x, y, length, horizontal))
                {
                    field.PlaceShip(x, y, length, horizontal);
                    placed = true;
                }
            }
        }
    }

    private void PlaceMines(GameField field, int numberOfMines)
    {
        for (int i = 0; i < numberOfMines; i++)
        {
            field.PlaceMine();
        }
    }

    private bool CanPlaceShip(GameField field, int x, int y, int length, bool horizontal)
    {
        for (int i = 0; i < length; i++)
        {
            int newX = x + (horizontal ? 0 : i);
            int newY = y + (horizontal ? i : 0);

            if (newX >= _settings.FieldSize || newY >= _settings.FieldSize || field.HasShip(newX, newY))
            {
                return false;
            }
        }
        return true;
    }

    private void EnemyTurn()
    {
        int x = _random.Next(0, _settings.FieldSize);
        int y = _random.Next(0, _settings.FieldSize);

        _playerField.Shoot(x, y);
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
                _enemyField.Shoot(x, y);

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