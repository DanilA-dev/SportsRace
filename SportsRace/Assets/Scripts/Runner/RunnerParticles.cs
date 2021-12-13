using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackEventParticleType
{
    WallHit, BoxPunch, SandJump, ObstacleHit, SwitchCharacter
}

public class RunnerParticles : MonoBehaviour
{
    [SerializeField] private List<ParticleType> trackTypeParticles = new List<ParticleType>();
    [SerializeField] private List<TrackEventParticle> trackEventParticles = new List<TrackEventParticle>();



    public void PlayByTrackType(SportType type)
    {
        foreach (var p in trackTypeParticles)
            p.Stop();

        for (int i = 0; i < trackTypeParticles.Count; i++)
        {
            if (trackTypeParticles[i].Type == type)
                trackTypeParticles[i].Play();
        }
    }

    public void StopAllLoopingParticles()
    {
        for (int i = 0; i < trackTypeParticles.Count; i++)
            trackTypeParticles[i].Stop();
    }

    public void PlayByTrackEvent(TrackEventParticleType type)
    {
        foreach (var p in trackEventParticles)
            p.Stop();

        for (int i = 0; i < trackEventParticles.Count; i++)
        {
            if (trackEventParticles[i].EventType == type)
                trackEventParticles[i].Play();
        }
    }

}

[System.Serializable]
public class ParticleType
{
    public SportType Type;
    public ParticleSystem Particle;

    public void Play()
    {
        Particle.gameObject.SetActive(true);
    }

    public void Stop()
    {
        Particle.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class TrackEventParticle
{
    public TrackEventParticleType EventType;
    public ParticleSystem Particle;

    public void Play()
    {
        Particle.gameObject.SetActive(true);
    }

    public void Stop()
    {
        Particle.gameObject.SetActive(false);
    }
}
