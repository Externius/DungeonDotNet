using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Generator.Models;

public class DungeonTile(int x, int y, int i = 0, int j = 0, int width = 0, int height = 0, Texture texture = Texture.Edge)
{
    public Texture Texture { get; set; } = texture;
    public int I { get; set; } = i;
    public int J { get; set; } = j;
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
    public int H { get; set; } = 0;
    public int G { get; set; }
    public int F { get; set; }
    public DungeonTile? Parent { get; set; }
    public string? Description { get; set; }
    public int Index { get; set; }
}