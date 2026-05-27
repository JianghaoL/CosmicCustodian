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
    
    /// <summary>
    /// This event is invoked when the box object is spawned.
    /// Use this to get the transform of the box object.
    /// </summary>
    public static readonly UnityEvent<Transform> OnBoxSpawned = new UnityEvent<Transform>();
    
    /// <summary>
    /// When box move is requested, this event is invoked.
    /// It passes a Vector2Int which can be used to validate
    /// the desired grid to move to.
    /// </summary>
    public static readonly UnityEvent<Vector2Int> OnBoxMoveRequested = new UnityEvent<Vector2Int>();
    
    /// <summary>
    /// Every time the box moves it invokes this event, which
    /// tells other scripts to check if a win/lose condition
    /// is reached.
    /// </summary>
    public static readonly UnityEvent<Vector2Int> OnWinCheckRequested = new UnityEvent<Vector2Int>();
    
    /// <summary>
    /// Invoked when player wins (box reaches destination)
    /// </summary>
    public static readonly UnityEvent OnGameWin = new UnityEvent();
    
    /// <summary>
    /// Invoked when player loses (impossible to retrieve the box)
    /// </summary>
    public static readonly UnityEvent OnGameLose = new UnityEvent();
    
    /// <summary>
    /// Invoked when player / box makes a move. This tells the scripts
    /// to update the grid type.
    /// </summary>
    public static readonly UnityEvent<Vector2Int, BlockType> OnGridBlockUpdate = new UnityEvent<Vector2Int, BlockType>();
    
    public static readonly UnityEvent OnRestartRequested = new UnityEvent();
    
    public static readonly UnityEvent OnUndoRequested = new UnityEvent();
    
    public static readonly UnityEvent<Vector2Int> RecordBoxPreviousCoord = new UnityEvent<Vector2Int>();
    
    public static readonly UnityEvent<Vector2Int> RecordBoxCurrentCoord = new UnityEvent<Vector2Int>();
    
    public static readonly UnityEvent<Vector2Int> RecordPlayerPreviousCoord = new UnityEvent<Vector2Int>();
    
    public static readonly UnityEvent<Vector2Int> RecordPlayerCurrentCoord = new UnityEvent<Vector2Int>();
    
    public static readonly UnityEvent<Vector3, Vector2Int> OnUndoPlayer =  new UnityEvent<Vector3, Vector2Int>();
    
    public static readonly UnityEvent<Vector3, Vector2Int> OnUndoBox =  new UnityEvent<Vector3, Vector2Int>();
}
