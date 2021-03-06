using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [HideInInspector]
    public float previousVolume;
    [Range(0,1)]
    public float volume;
    [Range(0.1f,3)]
    public float pitch;
    [HideInInspector]
    public AudioSource source;
    public bool loop;
}
