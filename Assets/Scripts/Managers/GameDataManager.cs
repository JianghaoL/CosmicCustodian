using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour, IInitializable
{
    public static GameDataManager Instance;
    
    private static Dictionary<Vector2Int, GridBlock> _gridBlocks;
    
    [SerializeField] private GameConfig config;

    public void InitializeOnAwake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
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

    public GameConfig GetConfig()
    {
        return config;
    }

    private void UpdateGridBlock(Vector2Int coord, BlockType newBlockType)
    {
        var gridBlock = new GridBlock(coord.x, coord.y, newBlockType);
        _gridBlocks[coord] = gridBlock;
    }
}
