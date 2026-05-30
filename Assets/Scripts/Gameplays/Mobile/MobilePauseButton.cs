using UnityEngine;
using UnityEngine.EventSystems;

public class MobilePauseButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private InputHandler inputHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!inputHandler)
        {
            Debug.LogError($"{nameof(inputHandler)} is not set");
            return;
        }
        
        inputHandler.RequestPause();
    }
}
