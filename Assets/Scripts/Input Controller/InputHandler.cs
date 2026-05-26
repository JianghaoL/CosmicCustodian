using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        var v = context.ReadValue<Vector2>();
        var dir = Vector2ToCoord(v);

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
        Debug.Log($"Player Move in direction {dir}");
        //TODO: Player Controller
    }

    public void RequestUndo()
    {
        Debug.Log("Undo action");
        //TODO: Player undo action
    }

    public void RequestRestart()
    {
        Debug.Log("Restart level");
        //TODO: Player restart level
    }

    public void RequestPause()
    {
        Debug.Log("Pause Game");
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


    private static Vector2Int Vector2ToCoord(Vector2 v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            return v.x > 0 ? Vector2Int.right : Vector2Int.left;

        if (Mathf.Abs(v.y) > 0.1f)
            return v.y > 0 ? Vector2Int.up : Vector2Int.down;

        return Vector2Int.zero;
    }
}
