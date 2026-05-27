using DG.Tweening;
using UnityEngine;

public class BoxMovementManager : MonoBehaviour, IInitializable
{
    private Transform _boxTransform;

    public void InitializeOnAwake()
    {
        GameEventsManager.OnBoxSpawned.AddListener(SetBoxTransform);
        GameEventsManager.OnBoxMoveRequested.AddListener(RequestMove);
        
        GameEventsManager.OnUndoBox.AddListener(MoveBox);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnBoxSpawned.RemoveListener(SetBoxTransform);
        GameEventsManager.OnBoxMoveRequested.RemoveListener(RequestMove);
        
        GameEventsManager.OnUndoBox.RemoveListener(MoveBox);
    }
    
    public bool ValidateBoxMoveCoord(Vector2Int dir)
    {
        var toGo = Vector2IntExtention.GetNextMoveFromDirection(dir, _boxTransform);
        
        var o = GameDataManager.GetGridBlock(toGo);
        var type = o.type;

        return type is not BlockType.Wall;
    }

    private void RequestMove(Vector2Int dir)
    {
        var requestedCoord = Vector2IntExtention.GetNextMoveFromDirection(dir, _boxTransform);
        var toGo = new Vector3(requestedCoord.x, 0.5f, requestedCoord.y);
        
        var currentCoord = Vector2IntExtention.Vector3ToCoord(_boxTransform.position);
        GameEventsManager.RecordBoxPreviousCoord.Invoke(currentCoord);
        GameEventsManager.RecordBoxCurrentCoord.Invoke(requestedCoord);
        
        GameEventsManager.OnWinCheckRequested.Invoke(requestedCoord);
        
        MoveBox(toGo, Vector2Int.zero);
    }

    private void MoveBox(Vector3 toGo, Vector2Int previous)
    {
        GameEventsManager.OnGridBlockUpdate.Invoke(Vector2IntExtention.Vector3ToCoord(toGo), BlockType.Box);
        
        if (previous != Vector2Int.zero) GameEventsManager.OnGridBlockUpdate.Invoke(previous, BlockType.Void);
        
        _boxTransform.DOMove(toGo, GameDataManager.Instance.GetConfig().moveDuration);
    }

    private void SetBoxTransform(Transform t)
    {
        _boxTransform = t;
    }
}
