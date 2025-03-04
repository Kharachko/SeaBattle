public class Ship
{
    public List<(int x, int y)> Cells { get; } = new List<(int x, int y)>();
    private List<(int x, int y)> _hits = new List<(int x, int y)>();

    public Ship(int x, int y, int length, bool horizontal)
    {
        for (int i = 0; i < length; i++)
        {
            if (horizontal)
            {
                Cells.Add((x, y + i));
            }
            else
            {
                Cells.Add((x + i, y));
            }
        }
    }

    public bool Hit(int x, int y)
    {
        if (Cells.Contains((x, y)))
        {
            _hits.Add((x, y));
            return true;
        }
        return false;
    }

    public bool IsSunk()
    {
        return Cells.All(cell => _hits.Contains(cell));
    }
}