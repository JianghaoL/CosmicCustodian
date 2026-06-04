using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour, IInitializable
{
    private AudioSource _audioSource;
    public void InitializeOnAwake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Fade(GameDataManager.Instance.GetConfig().cameraRotationDuration + 5f, FadeMode.FadeIn);
        
        GameEventsManager.OnGameWin.AddListener(SetFadeOut);
        GameEventsManager.OnGameQuit.AddListener(SetFadeOut);
        GameEventsManager.OnGameStart.AddListener(SetFadeOut);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnGameWin.RemoveListener(SetFadeOut);
        GameEventsManager.OnGameQuit.AddListener(SetFadeOut);
        GameEventsManager.OnGameStart.RemoveListener(SetFadeOut);
    }

    private void SetFadeOut()
    {
        _audioSource.Fade(GameDataManager.Instance.GetConfig().onWinLoadDelay, FadeMode.FadeOut);
    }
    
}
