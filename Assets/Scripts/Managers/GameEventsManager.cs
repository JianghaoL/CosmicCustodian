using System.Collections.Generic;
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
}
