using UnityEngine;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private VolumeSlider master;
    [SerializeField] private VolumeSlider music;
    [SerializeField] private VolumeSlider sfx;
    
    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    private void Start()
    {
        LoadSettings();
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        var masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        var musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        var sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        
        master.SetSlide(masterVolume);
        music.SetSlide(musicVolume);
        sfx.SetSlide(sfxVolume);
    }

    public void ResetToDefault()
    {
        PlayerPrefs.DeleteAll();
        
        master.SetSlide(1f);
        music.SetSlide(1f);
        sfx.SetSlide(1f);
    }
}
