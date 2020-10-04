using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Sound List")]
    public Sound[] sounds;
    public string currentMusic;

    public static AudioManager audioManager;
    public AudioMixer Mixer;
    public AudioMixerGroup MusicMixer;
    public AudioMixerGroup SoundMixer;

    [Header("Volume")]
    public float musicVolume;
    public float soundVolume;

    [Header("Pitch Range")]
    public float minPitch;
    public float maxPitch;

    void Awake()
    {
        if (audioManager == null)
            audioManager = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if (s.isMusic)
                s.source.outputAudioMixerGroup = MusicMixer;
            else
                s.source.outputAudioMixerGroup = SoundMixer;


            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        else if (currentMusic == name)
            return;
        else if (currentMusic != null)
            Stop(currentMusic);

        currentMusic = name;

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }
}
