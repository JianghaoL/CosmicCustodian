using UnityEngine;

[System.Serializable]
public struct GridBlock
{
    public Vector2Int coord;
    public BlockType type;

    public GridBlock(int x, int y, BlockType type)
    {
        this.coord = new Vector2Int(x, y);
        this.type = type;
    }

    public override string ToString()
    {
        return type.ToString();
    }
}
