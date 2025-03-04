using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class GameSettings
{
    public int FieldSize { get; set; }
    public List<int> PlayerShipLengths { get; set; }
    public List<int> EnemyShipLengths { get; set; }
    public int NumberOfMines { get; set; }
    public int NumberOfBuffs { get; set; }

    public static GameSettings Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Settings file not found: {filePath}");
        }

        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<GameSettings>(json);
    }
}