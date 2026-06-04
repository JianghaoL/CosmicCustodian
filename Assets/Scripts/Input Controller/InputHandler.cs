using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        var v = context.ReadValue<Vector2>();
        var dir = Vector2IntExtention.Vector2ToCoord(v);

        if (dir == Vector2Int.zero) return;
        RequestMove(dir);
    }

    public void OnUndo(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RequestUndo();
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        RequestRestart();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        RequestPause();
    }

    public void RequestMove(Vector2Int dir)
    {
        GameEventsManager.OnMoveRequested.Invoke(dir);
        GameEventsManager.OnMoveTutorial.Invoke(TutorialState.FirstMove);
    }

    public void RequestUndo()
    {
        Debug.Log("Undo action");
        GameEventsManager.OnUndoRequested.Invoke();
        GameEventsManager.OnUndoTutorial.Invoke(TutorialState.Undo);
        GameEventsManager.OnUIButtonClicked.Invoke();
    }

    public void RequestRestart()
    {
        Debug.Log("Restart level");
        GameEventsManager.OnRestartRequested.Invoke();
        GameEventsManager.OnUIButtonClicked.Invoke();
    }

    public void RequestPause()
    {
        Debug.Log("Pause Game");
        GameEventsManager.OnUIButtonClicked.Invoke();
        
        if (!GameManager.Instance.IsPaused()) GameEventsManager.OnPauseRequested.Invoke();
        else GameEventsManager.OnResumeRequested.Invoke();
    }

    public string GetCurrentControlScheme()
    {
        return playerInput ? playerInput.currentControlScheme : string.Empty;
    }

    private void RefreshControlSchemeLabel()
    {
        if (!playerInput) return;
        
        var label = GetCurrentControlScheme() == "KeyboardMouse" ? "WASD / Arrow Keys" : "Press Buttons";
        Debug.Log(label);
    }
    
    private void Start()
    {
        RefreshControlSchemeLabel();
    }
}
