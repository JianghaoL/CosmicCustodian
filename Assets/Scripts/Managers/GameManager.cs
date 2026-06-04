using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IInitializable
{
    public static GameManager Instance;

    private bool _isPaused;
    
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
        
        GameEventsManager.OnGameWin.AddListener(OnGameWin);
        GameEventsManager.OnPauseRequested.AddListener(HandleGamePause);
        GameEventsManager.OnResumeRequested.AddListener(HandleGameResume);
        GameEventsManager.OnGameQuit.AddListener(HandleGameQuit);
        
        _isPaused = false;
    }

    private void OnDestroy()
    {
        GameEventsManager.OnGameWin.RemoveListener(OnGameWin);
        GameEventsManager.OnPauseRequested.RemoveListener(HandleGamePause);
        GameEventsManager.OnResumeRequested.RemoveListener(HandleGameResume);
        GameEventsManager.OnGameQuit.RemoveListener(HandleGameQuit);
    }

    private void OnGameWin()
    {
        StartCoroutine(WaitToLoadNextLevel());
    }

    private IEnumerator WaitToLoadNextLevel()
    {
        var config = GameDataManager.Instance.GetConfig();
        yield return new WaitForSecondsRealtime(config.onWinLoadDelay);
        SceneManager.LoadScene(config.GetLevelToLoad());
    }
    
    private IEnumerator WaitToLoadMainMenu()
    {
        var config = GameDataManager.Instance.GetConfig();
        yield return new WaitForSecondsRealtime(config.fadeInDuration * 2);
        SceneManager.LoadScene("Main Page");
    }

    private void HandleGamePause()
    {
        //Time.timeScale = 0f;
        _isPaused = true;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, 0.5f);
    }

    private void HandleGameResume()
    {
        _isPaused = false;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.5f);
    }

    private void HandleGameQuit()
    {
        HandleGameResume();
        StartCoroutine(WaitToLoadMainMenu());
    }

    public bool IsPaused()
    {
        return _isPaused;
    }
}
