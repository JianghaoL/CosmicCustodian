using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : MonoBehaviour, IInitializable
{
    [SerializeField] private Image blackout;
    public void InitializeOnAwake()
    {
        blackout.color = new Color(blackout.color.r, blackout.color.g, blackout.color.b, 1f);
        blackout.DOFade(0f, GameDataManager.Instance.GetConfig().fadeInDuration);
        
        GameEventsManager.OnGameWin.AddListener(OnGameWin);
        GameEventsManager.OnGameStart.AddListener(OnGameStart);
        GameEventsManager.OnGameQuit.AddListener(OnGameStart);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnGameWin.RemoveListener(OnGameWin);
        GameEventsManager.OnGameStart.RemoveListener(OnGameStart);
        GameEventsManager.OnGameQuit.RemoveListener(OnGameStart);
    }

    private void OnGameWin()
    {
        StartCoroutine(WaitForCameraAnim());

        IEnumerator WaitForCameraAnim()
        {
            yield return new WaitForSecondsRealtime(GameDataManager.Instance.GetConfig().onWinLoadDelay - GameDataManager.Instance.GetConfig().onGameWinCamMoveDuration);
            blackout.DOFade(1f, GameDataManager.Instance.GetConfig().fadeInDuration);
        }
    }

    private void OnGameStart()
    {
        Debug.Log("Fading in...");
        blackout.DOFade(1f, GameDataManager.Instance.GetConfig().fadeInDuration);
    }
}
