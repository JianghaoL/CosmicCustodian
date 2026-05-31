using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IInitializable
{
    public void InitializeOnAwake()
    {
        GameEventsManager.OnGameWin.AddListener(OnGameWin);
        // GameEventsManager.OnGameWin.AddListener();
    }

    private void OnDestroy()
    {
        GameEventsManager.OnGameWin.RemoveListener(OnGameWin);
    }

    private void OnGameWin()
    {
        StartCoroutine(WaitToLoadNextLevel());
    }

    private IEnumerator WaitToLoadNextLevel()
    {
        Debug.Log("Loading next level...");
        var config = GameDataManager.Instance.GetConfig();
        yield return new WaitForSecondsRealtime(config.onWinWaitDelay);
        SceneManager.LoadScene(config.GetLevelToLoad());
    }
}
