using System.Collections.Generic;
using UnityEngine;

public class WinLoseManager : MonoBehaviour, IInitializable
{
    private bool _winLock;
    public void InitializeOnAwake()
    {
        _winLock = false;
        GameEventsManager.OnWinCheckRequested.AddListener(CheckWin);
        GameEventsManager.OnWinCheckRequested.AddListener(CheckLose);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnWinCheckRequested.RemoveListener(CheckWin);
        GameEventsManager.OnWinCheckRequested.RemoveListener(CheckLose);
    }
    
    private void CheckWin(Vector2Int coord)
    {
        var o = GameDataManager.GetGridBlock(coord);
        var type = o.type;
        
        if (type == BlockType.Destination)
        {
            _winLock = true;
            Debug.Log("Win!");
            GameEventsManager.OnGameWin.Invoke();
        }
    }
    
    private void CheckLose(Vector2Int coord)
    {
        if (_winLock) return;
        
        HashSet<Vector2Int> adjacent = new ();
        var o = GameDataManager.GetGridBlock(coord);
        
        int numNeighbors = 0;
        foreach (var dir in Vector2IntExtention.AllDirections.Directions)
        {
            var neighbor = Vector2IntExtention.GetNextMoveFromDirection(dir, coord);
            var neighborType = GameDataManager.GetGridBlock(neighbor).type;

            if (neighborType == BlockType.Wall)
            {
                adjacent.Add(dir);
                numNeighbors ++;
            }
        }
        
        // If a block has >2 wall neighbors, it's a dead corner.
        if (numNeighbors > 2) GameEventsManager.OnGameLose.Invoke();
        
        // Check if the box can be moved or if in a dead corner.
        // If, for example, the box has its left neighbor being the wall,
        // either having an up or down neighbor will make it impossible
        // to move the box.
        //
        // This checks if, for any given wall, there is an adjacent wall
        // that prevents the box from moving.
        bool lose = false;
        foreach (var p in adjacent)
        {
            if (p == Vector2Int.up || p == Vector2Int.down)
            {
                lose = adjacent.Contains(Vector2Int.left) || adjacent.Contains(Vector2Int.right);
            }
            else if (p == Vector2Int.left || p == Vector2Int.right)
            {
                lose = adjacent.Contains(Vector2Int.up) || adjacent.Contains(Vector2Int.down);
            }
            
            if (lose) break;
        }
        
        if (lose) Debug.Log("Lose!");
        if (lose) GameEventsManager.OnGameLose.Invoke();
    }
}
