using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private Dictionary<AudioSource, float> _activeAudioSource;
    private Dictionary<AudioClip, AudioSource> _addedSources;

    private void Awake()
    {
        _activeAudioSource = new Dictionary<AudioSource, float>();
        _addedSources = new Dictionary<AudioClip, AudioSource>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {
        if (_activeAudioSource.Count <= 0) return;
        //
        // // If there is active audio source in the dictionary, see if it has reached the end.
        // foreach (var source in _activeAudioSource)
        // {
        //     source.Value = Time.deltaTime;
        // }
    }

    public bool Play(AudioClip clip,
        float volume = 1f,
        float pitch = 1f,
        bool loop = false,
        AudioMixerGroup sfxGroup = null)
    {
        bool result;
        try
        {
            if (!_addedSources.TryGetValue(clip, out AudioSource source))
            {
                source = gameObject.AddComponent<AudioSource>();
                _addedSources.Add(clip, source);
            }
            
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;
            source.playOnAwake = false;
            source.outputAudioMixerGroup = sfxGroup;
            source.Play();
            if (!loop) _activeAudioSource.Add(source, 0);
            result = true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            result = false;
        }
        return result;
    }
}
