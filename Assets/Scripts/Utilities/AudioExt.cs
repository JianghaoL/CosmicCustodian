using DG.Tweening;
using UnityEngine;

public static class AudioExt
{
    public static void SetVolume(this AudioSource audioSource, float volume)
    {
        audioSource.volume = volume;
    }

    public static void SetPitch(this AudioSource audioSource, float pitch)
    {
        audioSource.pitch = pitch;
    }

    public static void Fade(this AudioSource audioSource, float fadeTime, FadeMode fm = FadeMode.FadeIn)
    {
        float presetVolume = audioSource.volume;
        switch (fm)
        {
            case FadeMode.FadeIn:
                if (audioSource.isPlaying) audioSource.Stop();
                audioSource.volume = 0f;
                audioSource.Play();
                audioSource.DOFade(presetVolume, fadeTime);
                break;
            case FadeMode.FadeOut:
                audioSource.DOFade(0f, fadeTime);
                break;
        }
    }
}

public enum FadeMode
{
    FadeIn,
    FadeOut
}
