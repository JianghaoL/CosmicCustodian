using System.Collections;
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
        GameEventsManager.OnMapConstructed.AddListener(ShowUi);
    }

    private void ShowUi()
    {
        StartCoroutine(WaitForCamera());

        IEnumerator WaitForCamera()
        {
            yield return new WaitForSecondsRealtime(GameDataManager.Instance.GetConfig().cameraRotationDuration);
            mobileMoveUi.SetActive(true);
        }
    }
    
}
