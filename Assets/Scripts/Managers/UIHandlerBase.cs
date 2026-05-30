using System.Collections;
using TMPro;
using UnityEngine;

public class UIHandlerBase : MonoBehaviour
{
    [Header("Task Bar Body")]
    protected GameObject taskBar;
    protected TextMeshProUGUI promptText;
    protected RectTransform promptBackgroundRect;
    protected float topPadding = 20f;
    protected float bottomPadding = 20f;
    protected GameObject mobileMoveUi;

    
    public virtual void Initialize(TextMeshProUGUI promptText,
        RectTransform promptBackgroundRect,
        float topPadding,
        float bottomPadding,
        GameObject mobileMoveUi,
        GameObject taskbar)
    {
        this.promptText = promptText;
        this.promptBackgroundRect = promptBackgroundRect;
        this.topPadding = topPadding;
        this.bottomPadding = bottomPadding;
        this.mobileMoveUi = mobileMoveUi;
        this.taskBar = taskbar;
        taskbar.SetActive(false);
        
        GameEventsManager.OnMapConstructed.AddListener(ShowTaskBar);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnMapConstructed.RemoveListener(ShowTaskBar);
    }

    private void ShowTaskBar()
    {
        taskBar.SetActive(true);
    }

    public void SetPromptText(string text)
    {
        promptText.text = text;
        UpdateBackgroundHeight();
    }

    private void UpdateBackgroundHeight()
    {
        promptText.ForceMeshUpdate();
        
        var height = promptText.GetPreferredValues(
            promptText.text,
            promptText.rectTransform.rect.width,
            Mathf.Infinity
        ).y;
        
        //var width = promptBackgroundRect.sizeDelta.x;
        
        promptBackgroundRect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            height
        );
    }
}
