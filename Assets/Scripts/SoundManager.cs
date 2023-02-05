using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public enum Sfx
    {
        Hit,
        Pulling,
        Pulled,
        Landing,
        Throw
    }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip gameSound;

    [Header("SFX")] [SerializeField] private List<AudioClip> carrotHit;
    [SerializeField] private List<AudioClip> carrotPulling;
    [SerializeField] private List<AudioClip> carrotPulled;
    [SerializeField] private List<AudioClip> carrotLanding;
    [SerializeField] private List<AudioClip> carrotThrow;

    private Dictionary<Sfx, List<AudioClip>> sfxMap = new();

    private void Awake()
    {
        sfxMap.Clear();
        sfxMap.Add(Sfx.Hit, carrotHit);
        sfxMap.Add(Sfx.Pulling, carrotPulling);
        sfxMap.Add(Sfx.Pulled, carrotPulled);
        sfxMap.Add(Sfx.Landing, carrotLanding);
        sfxMap.Add(Sfx.Throw, carrotThrow);
    }

    private void Start()
    {
        musicSource.clip = gameSound;
        musicSource.loop = true;
        musicSource.Play();
    }

    public AudioSource PlaySfxLoop(Sfx sound)
    {
        var sfx = GetSfxClip(sound);
        var sfxSource = gameObject.AddComponent<AudioSource>();

        sfxSource.clip = sfx;
        sfxSource.loop = true;
        sfxSource.Play();

        return sfxSource;
    }

    public void StopSfxLoop(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.loop = false;
        source.Stop();
        Destroy(source);
    }

    public void PlaySfx(Sfx sound)
    {
        var sfx = GetSfxClip(sound);
        musicSource.PlayOneShot(sfx, 3.5f);
    }

    private AudioClip GetSfxClip(Sfx sound)
    {
        var sounds = sfxMap[sound];
        return sounds[Random.Range(0, sounds.Count)];
    }
}