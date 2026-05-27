using DG.Tweening;
using UnityEngine;

public class BoxMovementManager : MonoBehaviour, IInitializable
{
    private Transform _boxTransform;

    public void InitializeOnAwake()
    {
        GameEventsManager.OnBoxSpawned.AddListener(SetBoxTransform);
        GameEventsManager.OnBoxMoveRequested.AddListener(RequestMove);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnBoxSpawned.RemoveListener(SetBoxTransform);
        GameEventsManager.OnBoxMoveRequested.RemoveListener(RequestMove);
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
        
        GameEventsManager.OnWinCheckRequested.Invoke(requestedCoord);
        GameEventsManager.OnGridBlockUpdate.Invoke(requestedCoord, BlockType.Box);
        
        _boxTransform.DOMove(toGo, GameDataManager.Instance.GetConfig().moveDuration);
        
    }

    private void SetBoxTransform(Transform t)
    {
        _boxTransform = t;
    }
}
