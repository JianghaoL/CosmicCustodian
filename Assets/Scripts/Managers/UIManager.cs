using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour, IInitializable
{
    public static UIManager Instance;
    
    [Header("References")]
    [SerializeField] private MobileUIHandler mobileUIHandler;
    [SerializeField] private DesktopUIHandler desktopUIHandler;
    
    [Header("Mobile Move UI")]
    [SerializeField] private GameObject mobileMoveUi;

    [Header("Task Bar Body")] 
    [SerializeField] private GameObject taskBar;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private RectTransform promptBackgroundRect;
    [SerializeField] private float topPadding = 20f;
    [SerializeField] private float bottomPadding = 20f;
    
    private UIHandlerBase _uiHandler;
    
    public void InitializeOnAwake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        var p = PlatformManager.Instance.GetPlatform();
        if (p == PlatformManager.Platform.Desktop)
        {
            _uiHandler = desktopUIHandler;
        }
        else
        {
            _uiHandler = mobileUIHandler;
        }
        _uiHandler.Initialize(
            promptText, 
            promptBackgroundRect, 
            topPadding, 
            bottomPadding,
            mobileMoveUi,
            taskBar);
    }
    
    public void SetPromptText(string prompt)
    {
        _uiHandler.SetPromptText(prompt);
    }
}
