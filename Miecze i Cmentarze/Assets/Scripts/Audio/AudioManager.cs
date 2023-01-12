using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds, music;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
    }

    private void Start()
    {
        PlayMusic("theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void PlayMusic(string name)
    {
        Sound m = Array.Find(music, music => music.name == name);
        m.source.Play();
    }

    public void StopMusic(string name)
    {
        Sound m = Array.Find(music, music => music.name == name);
        m.source.Play();
    }

    public void SetVolume(float value)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = value;
        }
    }

    public void SetMusicVolume(float value)
    {
        foreach (Sound m in music)
        {
            m.source.volume = value;
        }
    }
}
