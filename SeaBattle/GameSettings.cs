using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class GameSettings
{
    public int FieldSize { get; set; }
    public required List<int> PlayerShipLengths { get; set; }
    public required List<int> EnemyShipLengths { get; set; }
    public int NumberOfMines { get; set; }
    public int NumberOfBuffs { get; set; }
    public string ShipColor { get; set; } = "#808080";
    public string MineColor { get; set; } = "#FF0000";
    public string HitColor { get; set; } = "#FF4500";
    public string MissColor { get; set; } = "#FFD700";
    public string DefaultColor { get; set; } = "#FFFFFF";
    public string ExplosionColor { get; set; } = "#FFA500";
    public string FieldColor { get; set; } = "#1E90FF";

    public static GameSettings Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Settings file not found: {filePath}");
        }

        var json = File.ReadAllText(filePath);
        var settings = JsonConvert.DeserializeObject<GameSettings>(json);

        if (settings == null)
        {
            throw new InvalidOperationException("Failed to load game settings.");
        }

        return settings;
    }

    public static ConsoleColor HexToConsoleColor(string hexColor)
    {
        if (string.IsNullOrEmpty(hexColor) || hexColor[0] != '#' || hexColor.Length != 7)
        {
            throw new ArgumentException("Invalid hex color format. Expected format: #RRGGBB");
        }

        int r = Convert.ToInt32(hexColor.Substring(1, 2), 16);
        int g = Convert.ToInt32(hexColor.Substring(3, 2), 16);
        int b = Convert.ToInt32(hexColor.Substring(5, 2), 16);

        int index = (r > 128 | g > 128 | b > 128) ? 8 : 0; // Bright bit
        index |= (r > 64) ? 4 : 0; // Red bit
        index |= (g > 64) ? 2 : 0; // Green bit
        index |= (b > 64) ? 1 : 0; // Blue bit

        return (ConsoleColor)index;
    }
}