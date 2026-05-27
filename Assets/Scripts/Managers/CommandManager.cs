using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandManager : MonoBehaviour, IInitializable
{
    private Vector2Int _previousPlayerCoord;
    private Vector2Int _currentPlayerCoord;
    private Vector2Int _previousBoxCoord;
    private Vector2Int _currentBoxCoord;

    private bool _hasUndone;
    private int _moveCount;
    private bool _isPlayerOnlyMove;
    
    public void InitializeOnAwake()
    {
        GameEventsManager.OnRestartRequested.AddListener(OnRestartRequested);
        GameEventsManager.OnUndoRequested.AddListener(OnUndoRequested);
        
        GameEventsManager.OnMoveRequested.AddListener(RefreshUndo);
        
        GameEventsManager.RecordPlayerPreviousCoord.AddListener(RecordPlayerPreviousCoord);
        GameEventsManager.RecordPlayerCurrentCoord.AddListener(RecordPlayerCurrentCoord);
        GameEventsManager.RecordBoxPreviousCoord.AddListener(RecordBoxPreviousCoord);
        GameEventsManager.RecordBoxCurrentCoord.AddListener(RecordBoxCurrentCoord);
        
        _isPlayerOnlyMove = false;
        _hasUndone = false;
    }

    public void OnDestroy()
    {
        GameEventsManager.OnRestartRequested.RemoveListener(OnRestartRequested);
        GameEventsManager.OnUndoRequested.RemoveListener(OnUndoRequested);
        
        GameEventsManager.OnMoveRequested.RemoveListener(RefreshUndo);
        
        GameEventsManager.RecordPlayerPreviousCoord.RemoveListener(RecordPlayerPreviousCoord);
        GameEventsManager.RecordPlayerCurrentCoord.RemoveListener(RecordPlayerCurrentCoord);
        GameEventsManager.RecordBoxPreviousCoord.RemoveListener(RecordBoxPreviousCoord);
        GameEventsManager.RecordBoxCurrentCoord.RemoveListener(RecordBoxCurrentCoord);
    }

    private void OnRestartRequested()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RefreshUndo(Vector2Int dir)
    {
        _hasUndone = false;
        Debug.Log(_isPlayerOnlyMove);
    }

    private void RecordPlayerPreviousCoord(Vector2Int previousCoord)
    {
        _previousPlayerCoord = previousCoord;
    }

    private void RecordPlayerCurrentCoord(Vector2Int currentCoord)
    {
        _currentPlayerCoord = currentCoord;
        _isPlayerOnlyMove = true;
    }

    private void RecordBoxPreviousCoord(Vector2Int previousBoxCoord)
    {
        _previousBoxCoord = previousBoxCoord;
        StartCoroutine(WaitForPoint1Sec());

        IEnumerator WaitForPoint1Sec()
        {
            yield return new WaitForSecondsRealtime(0.01f);
            _isPlayerOnlyMove = false;
        }
    }

    private void RecordBoxCurrentCoord(Vector2Int currentBoxCoord)
    {
        _currentBoxCoord = currentBoxCoord;
    }

    private void OnUndoRequested()
    {
        if (_hasUndone) return;
        
        _hasUndone = true;
        
        if (_previousPlayerCoord != Vector2Int.zero) 
            GameEventsManager.OnUndoPlayer.Invoke(Vector2IntExtention.CoordToVector3(_previousPlayerCoord), _currentPlayerCoord);
        
        if (_previousBoxCoord != Vector2Int.zero && !_isPlayerOnlyMove) 
            GameEventsManager.OnUndoBox.Invoke(Vector2IntExtention.CoordToVector3(_previousBoxCoord),  _currentBoxCoord);
    }
    
}
