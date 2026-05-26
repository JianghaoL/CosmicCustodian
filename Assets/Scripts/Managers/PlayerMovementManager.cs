using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.5f;
    
    private Transform _playerTransform;
    private bool _moveLock;
    
    private void Awake()
    {
        GameEventsManager.OnMoveRequested.AddListener(RequestMove);
        GameEventsManager.OnPlayerSpawned.AddListener(SetPlayerTransform);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnMoveRequested.RemoveListener(RequestMove);
        GameEventsManager.OnPlayerSpawned.RemoveListener(SetPlayerTransform);
    }

    private void Start()
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
        var playerCoord = new Vector2Int(Mathf.RoundToInt(_playerTransform.position.x), Mathf.RoundToInt(_playerTransform.position.z));
        var requestedCoord = playerCoord + dir;
        
        // Guard clause. Early return if requested coord not valid.
        if (!ValidateMoveCoord(requestedCoord)) return;
        
        var toGo = new Vector3(requestedCoord.x, 0.5f, requestedCoord.y);
        
        // Invoke the update block event. Old grid => void, new grid => player.
        GameEventsManager.OnGridBlockUpdate.Invoke(playerCoord, BlockType.Void);
        GameEventsManager.OnGridBlockUpdate.Invoke(requestedCoord, BlockType.Player);
        
        _playerTransform.DOMove(toGo, moveDuration);
    }

    private IEnumerator LockTimer()
    {
        _moveLock = true;
        yield return new WaitForSeconds(moveDuration + 0.01f);
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

        return type == BlockType.Box || type == BlockType.Destination || type == BlockType.Void;
    }

    private void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }
}
