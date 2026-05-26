using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private static Dictionary<Vector2Int, GridBlock> _gridBlocks;

    private void Awake()
    {
        GameEventsManager.OnLevelDataLoaded.AddListener(OnLevelDataLoaded);
        GameEventsManager.OnGridBlockUpdate.AddListener(UpdateGridBlock);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnLevelDataLoaded.RemoveListener(OnLevelDataLoaded);
        GameEventsManager.OnGridBlockUpdate.RemoveListener(UpdateGridBlock);
    }

    public static Dictionary<Vector2Int, GridBlock> GetAllBlocks()
    {
        return _gridBlocks;
    }

    private void OnLevelDataLoaded(Dictionary<Vector2Int, GridBlock> coordToGridBlock)
    {
        _gridBlocks = coordToGridBlock;
    }

    public static GridBlock GetGridBlock(Vector2Int coord)
    {
        return _gridBlocks[coord];
    }

    private void UpdateGridBlock(Vector2Int coord, BlockType newBlockType)
    {
        var gridBlock = new GridBlock(coord.x, coord.y, newBlockType);
        _gridBlocks[coord] = gridBlock;
    }
}
