using UnityEngine;
using UnityEngine.EventSystems;

public class MobileUndoButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private InputHandler inputHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!inputHandler)
        {
            Debug.LogError($"{nameof(inputHandler)} is not set");
            return;
        }
        
        inputHandler.RequestUndo();
    }
}
