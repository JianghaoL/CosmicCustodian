using TMPro;
using UnityEngine;

public class MobileUIHandler : UIHandlerBase
{
    public override void Initialize(TextMeshProUGUI promptText,
        RectTransform promptBackgroundRect,
        float topPadding,
        float bottomPadding,
        GameObject mobileMoveUi, GameObject taskbar)
    {
        base.Initialize(promptText, promptBackgroundRect, topPadding, bottomPadding, mobileMoveUi, taskbar);
        mobileMoveUi.SetActive(true);
    }
    
}
