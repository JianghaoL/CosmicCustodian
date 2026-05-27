using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxMovementManager))]
public class PlayerMovementManager : MonoBehaviour, IInitializable
{
    private BoxMovementManager _boxMovementManager;
    
    private Transform _playerTransform;
    private bool _moveLock;
    
    public void InitializeOnAwake()
    {
        _boxMovementManager = GetComponent<BoxMovementManager>();
        
        GameEventsManager.OnMoveRequested.AddListener(RequestMove);
        GameEventsManager.OnPlayerSpawned.AddListener(SetPlayerTransform);
        
        GameEventsManager.OnUndoPlayer.AddListener(MovePlayer);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnMoveRequested.RemoveListener(RequestMove);
        GameEventsManager.OnPlayerSpawned.RemoveListener(SetPlayerTransform);
        
        GameEventsManager.OnUndoPlayer.RemoveListener(MovePlayer);
    }

    public void InitializeOnStart()
    {
        _moveLock = false;
    }

    
    
    /// <summary>
    /// Request a move action. Automatically validates destination.
    /// </summary>
    /// <param name="dir">The desired moving direction</param>
    private void RequestMove(Vector2Int dir)
    {
        if (_moveLock) return;

        StartCoroutine(LockTimer()); // Lock player's movement temporarily (for the duration of the movement)
        
        // Create a coord struct using the current player position
        
        // var playerCoord = new Vector2Int(Mathf.RoundToInt(_playerTransform.position.x), Mathf.RoundToInt(_playerTransform.position.z));
        // var requestedCoord = playerCoord + dir;
        
        var playerCoord = new Vector2Int(Mathf.RoundToInt(_playerTransform.position.x), Mathf.RoundToInt(_playerTransform.position.z));
        var requestedCoord = Vector2IntExtention.GetNextMoveFromDirection(dir, _playerTransform);
        
        // Guard clause. Early return if requested coord not valid.
        if (!ValidateMoveCoord(requestedCoord)) return;
        
        if (GameDataManager.GetGridBlock(requestedCoord).type is BlockType.Box)
        {
            if (!_boxMovementManager.ValidateBoxMoveCoord(dir)) return;
            
            // Invoke box move request if the box is in front of the player
            GameEventsManager.OnBoxMoveRequested.Invoke(dir);
        }
        
        var toGo = new Vector3(requestedCoord.x, 0.5f, requestedCoord.y);
        
        GameEventsManager.RecordPlayerPreviousCoord.Invoke(playerCoord);
        GameEventsManager.RecordPlayerCurrentCoord.Invoke(requestedCoord);
        
        MovePlayer(toGo, playerCoord);
    }

    private void MovePlayer(Vector3 toGo, Vector2Int currentPlayerCoord)
    {
        // Move player
        
        // Invoke the update block event. Old grid => void, new grid => player.
        GameEventsManager.OnGridBlockUpdate.Invoke(currentPlayerCoord, BlockType.Void);
        GameEventsManager.OnGridBlockUpdate.Invoke(Vector2IntExtention.Vector3ToCoord(toGo), BlockType.Player);
        
        _playerTransform.DOMove(toGo, GameDataManager.Instance.GetConfig().moveDuration);
    }

    private IEnumerator LockTimer()
    {
        _moveLock = true;
        yield return new WaitForSeconds(GameDataManager.Instance.GetConfig().moveDuration + 0.01f);
        _moveLock = false;
    }

    
    
    /// <summary>
    /// Validates a coord and see if it is a legal tile to move to.
    /// </summary>
    /// <param name="coord">The requested coord</param>
    /// <returns>if the coord is valid</returns>
    private bool ValidateMoveCoord(Vector2Int coord)
    {
        var o = GameDataManager.GetGridBlock(coord);
        var type = o.type;

        return type is BlockType.Destination or BlockType.Void or BlockType.Box;
    }

    private void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        GameEventsManager.RecordPlayerCurrentCoord.Invoke(Vector2IntExtention.Vector3ToCoord(_playerTransform.position));
    }
}
