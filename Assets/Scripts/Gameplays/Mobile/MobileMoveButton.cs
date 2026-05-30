using UnityEngine;
using UnityEngine.EventSystems;

public class MobileMoveButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private Vector2Int direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!inputHandler)
        {
            Debug.LogError($"{nameof(inputHandler)} is not set");
            return;
        }
        
        inputHandler.RequestMove(direction);
    }
}
