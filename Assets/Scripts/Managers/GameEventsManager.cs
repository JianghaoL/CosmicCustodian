using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.Events;

public static class GameEventsManager
{
    /// <summary>
    /// This event is invoked when the LevelLoader script finishes loading level data.
    /// It carries a dictionary, which can be used to construct
    /// a game map.
    /// </summary>
    public static readonly UnityEvent<Dictionary<Vector2Int, GridBlock>> OnLevelDataLoaded = new UnityEvent<Dictionary<Vector2Int, GridBlock>>();
    
    /// <summary>
    /// This event marks that the map construction is completed.
    /// Upper level managers can subscribe to this event to manipulate game state.
    /// </summary>
    public static readonly UnityEvent OnMapConstructed = new UnityEvent();
    
    /// <summary>
    /// This event is invoked when player requests a move action.
    /// It carries a Vector2Int = direction the player is moving in.
    /// (unit = 1)
    /// </summary>
    public static readonly UnityEvent<Vector2Int> OnMoveRequested = new UnityEvent<Vector2Int>();
    
    /// <summary>
    /// This event is invoked when the player object is spawned.
    /// Use this to get the transform of the player object.
    /// </summary>
    public static readonly UnityEvent<Transform> OnPlayerSpawned = new UnityEvent<Transform>();
    
    public static readonly UnityEvent<Vector2Int, BlockType> OnGridBlockUpdate = new UnityEvent<Vector2Int, BlockType>();
}
