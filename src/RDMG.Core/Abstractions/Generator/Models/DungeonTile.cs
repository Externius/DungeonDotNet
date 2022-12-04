using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Generator.Models;

public class DungeonTile
{
    public Textures Texture { get; set; }
    public int I { get; set; }
    public int J { get; set; }
    public int X { get; }
    public int Y { get; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int H { get; set; }
    public int G { get; set; }
    public int F { get; set; }
    public DungeonTile Parent { get; set; }
    public string Description { get; set; }
    public int Index { get; set; }

    public DungeonTile(int x, int y, int i = 0, int j = 0, int width = 0, int height = 0, Textures texture = Textures.Edge)
    {
        X = x;
        Y = y;
        I = i;
        J = j;
        Width = width;
        Height = height;
        Texture = texture;
        H = 0;
    }
}