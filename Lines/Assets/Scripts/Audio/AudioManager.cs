using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds = new List<Sound>();
    public static AudioManager Instance { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    void Start()
    {
        Play("Theme", true);
    }
    public void Play(string name, bool isDelay)
    {
        if (isDelay)
        {
            Sound sound = sounds.Find(s => s.name == name);
            if (sound != null && sound.source.isPlaying == false)
            {
                sound.source.Play();
            }
            if (sound == null)
            {
                Debug.LogWarning("Sound : " + name + " not found !");
            }
        }
        else
        {
            Sound sound = sounds.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Play();
            }
            if (sound == null)
            {
                Debug.LogWarning("Sound : " + name + " not found !");
            }
        }
    }

    public void Stop(string name, bool isForce)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (isForce)
        {
            if (sound != null)
            {
                sound.source.Stop();
            }
        }
        else
        {
            if (sound != null && sound.source.isPlaying == true)
            {
                sound.source.Stop();
            }
        }
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            Stop(s.name, false);
        }
    }
    public void ForceStopAll()
    {
        foreach (Sound s in sounds)
        {
            Stop(s.name, true);
        }
    }
    public void MuteAll()
    {
        foreach (Sound sound in sounds)
        {
            sound.previousVolume = sound.volume;
            sound.volume = 0;
            sound.source.volume = sound.volume;
        }
    }
    public void Mute(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.previousVolume = sound.volume;
            sound.volume = 0;
            sound.source.volume = sound.volume;
        }
    }
    public void UnmuteAll()
    {
        foreach (Sound sound in sounds)
        {
            sound.volume = sound.previousVolume;
            sound.source.volume = sound.volume;
        }
    }
    public void Unmute(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.volume = sound.previousVolume;
            sound.source.volume = sound.volume;
        }
    }
}
