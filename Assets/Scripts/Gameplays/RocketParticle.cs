using System;
using UnityEngine;

public class RocketParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem coreFlame;
    [SerializeField] private ParticleSystem outerFlame;
    
    private bool _shouldPlay;

    private void Awake()
    {
        StopPlay();
        _shouldPlay = false;
        GameEventsManager.OnMapConstructed.AddListener(StartPlay);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnMapConstructed.RemoveListener(StartPlay);
    }

    private void StartPlay()
    {
        if (_shouldPlay)
        {
            coreFlame.Play();
            outerFlame.Play();
        }
    }

    public void StopPlay()
    {
        coreFlame.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        outerFlame.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    
    public void SetShouldPlay(bool shouldPlay)
    {
        _shouldPlay = shouldPlay;
    }
}
