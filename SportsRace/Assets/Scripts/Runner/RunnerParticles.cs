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
    [SerializeField] private List<RunnerSpecialParticle> runnerSpecialParticle = new List<RunnerSpecialParticle>();


    public void PlayByTrackType(SportType type)
    {
        if (trackTypeParticles.Count <= 0)
            return;

        foreach (var p in trackTypeParticles)
            p.Stop();

        for (int i = 0; i < trackTypeParticles.Count; i++)
        {
            if (trackTypeParticles[i].Type == type)
                trackTypeParticles[i].Play();
        }
    }


    public void PlayRunnerSpecial(SportType type)
    {
        if (runnerSpecialParticle.Count <= 0)
            return;

        foreach (var p in runnerSpecialParticle)
            p.Stop();

        for (int i = 0; i < runnerSpecialParticle.Count; i++)
        {
            if (runnerSpecialParticle[i].SpecialType == type)
                runnerSpecialParticle[i].Play();
        }
    }


    public void PlayByTrackEvent(TrackEventParticleType type)
    {
        if (trackEventParticles.Count <= 0)
            return;

        foreach (var p in trackEventParticles)
            p.Stop();

        for (int i = 0; i < trackEventParticles.Count; i++)
        {
            if (trackEventParticles[i].EventType == type)
                trackEventParticles[i].Play();
        }
    }

    public void StopRunnerSpecialParticles()
    {
        for (int i = 0; i < runnerSpecialParticle.Count; i++)
            runnerSpecialParticle[i].Stop();
    }

    public void StopTrackTypeParticles()
    {
        for (int i = 0; i < trackTypeParticles.Count; i++)
            trackTypeParticles[i].Stop();
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

[System.Serializable]
public class RunnerSpecialParticle
{
    public SportType SpecialType;
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
