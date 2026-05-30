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
        UIManager.Instance.SetPromptText("Oh hey you are moving this way! You are right!");
        GameEventsManager.OnMoveRequested.Invoke(dir);
    }

    public void RequestUndo()
    {
        Debug.Log("Undo action");
        UIManager.Instance.SetPromptText("Right, right. You just un-did your move. That's impressive.");
        GameEventsManager.OnUndoRequested.Invoke();
    }

    public void RequestRestart()
    {
        Debug.Log("Restart level");
        GameEventsManager.OnRestartRequested.Invoke();
    }

    public void RequestPause()
    {
        Debug.Log("Pause Game");
        UIManager.Instance.SetPromptText("Okay well... You just paused the game. How are you going to resume it?");
        //TODO: Pause game 
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
