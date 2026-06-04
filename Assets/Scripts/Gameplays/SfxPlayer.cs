using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    private void Awake()
    {
        GameEventsManager.OnGameWin.AddListener(PlayShipFlyBySound);
        GameEventsManager.OnUIButtonClicked.AddListener(PlayUIButtonClick);
        GameEventsManager.OnShowTutorial.AddListener(PlayTutorialNotification);
        GameEventsManager.OnBoxArriveDestination.AddListener(PlayPlaceBox);
        GameEventsManager.MoveRequested.AddListener(PlayMove);
        GameEventsManager.OnPlatformAssemble.AddListener(PlayPlatformAssemble);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnGameWin.RemoveListener(PlayShipFlyBySound);
        GameEventsManager.OnUIButtonClicked.RemoveListener(PlayUIButtonClick);
        GameEventsManager.OnShowTutorial.RemoveListener(PlayTutorialNotification);
        GameEventsManager.OnBoxArriveDestination.RemoveListener(PlayPlaceBox);
        GameEventsManager.MoveRequested.RemoveListener(PlayMove);
        GameEventsManager.OnPlatformAssemble.RemoveListener(PlayPlatformAssemble);
    }
    
    private void PlayShipFlyBySound()
    {
        AudioManager.Instance.Play(SfxHolder.Instance.shipFlyBy, 0.7f, 1f, false, SfxHolder.Instance.sfxGroup);
    }

    private void PlayUIButtonClick()
    {
        AudioManager.Instance.Play(SfxHolder.Instance.uiButtonClick, 1f, 1f, false, SfxHolder.Instance.sfxGroup);
    }

    private void PlayTutorialNotification()
    {
        AudioManager.Instance.Play(SfxHolder.Instance.notification, 1f, 1f, false, SfxHolder.Instance.sfxGroup);
    }

    private void PlayPlaceBox()
    {
        AudioManager.Instance.Play(SfxHolder.Instance.placeBox, 0.7f, 1f, false, SfxHolder.Instance.sfxGroup);
    }

    private void PlayMove()
    {
        var pitch = Random.Range(0.8f, 1.2f);
        AudioManager.Instance.Play(SfxHolder.Instance.move, 0.7f, pitch, false, SfxHolder.Instance.sfxGroup);
    }

    private void PlayPlatformAssemble()
    {
        AudioManager.Instance.Play(SfxHolder.Instance.platformAssemble, 0.7f, 1f, false, SfxHolder.Instance.sfxGroup);
    }
}
