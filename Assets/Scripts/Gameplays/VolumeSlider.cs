using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string Volume = "Volume";
    
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        Debug.Log(_slider != null);
        Debug.Log(Volume + " " + PlayerPrefs.GetFloat(Volume));
        _slider.onValueChanged.AddListener(SetVolume);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        var dB = value == 0f ? -80f : Mathf.Log10(value) * 20f;
        audioMixer.SetFloat(Volume, dB);
        PlayerPrefs.SetFloat(Volume, value);
        PlayerPrefs.Save();
    }

    public void SetSlide(float value)
    {
        value = Mathf.Clamp01(value);
        _slider.value = value;
        SetVolume(value);
    }
}
