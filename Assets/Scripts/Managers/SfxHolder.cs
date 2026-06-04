using UnityEngine;
using UnityEngine.Audio;

public class SfxHolder : MonoBehaviour
{
    public static SfxHolder Instance;

    public AudioMixerGroup sfxGroup;
    
    [Header("Gameplay")] 
    public AudioClip shipFlyBy;
    public AudioClip placeBox;
    public AudioClip platformAssemble;

    [Header("UI")] 
    public AudioClip uiButtonClick;
    public AudioClip notification;
    public AudioClip move;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
