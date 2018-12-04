using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        InitializeSoundClips();
    }

    private void Start()
    {
        PlayClip("MainTheme");
        PlayClipWithDelay("CrowdNoise", 2f);
        PlayClipWithDelay("CrowdNoise2", 5f);
    }

    void InitializeSoundClips()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.isLooping;
        }
    }

    public void PlayClip(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (s.source == null)
        {
            Debug.LogWarning("Sound Source for: " + name + " not found");
            return;
        }
        s.source.Play();
    }

    public void PlayClipWithDelay(string name, float delayTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.PlayDelayed(delayTime);
    }

    public void StopClip(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Stop();
    }

    public void StopAllClips()
    {
        foreach(AudioSource s in transform)
        {
            s.Stop();
        }
    }

}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0,1)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;

    public bool isLooping;

    [HideInInspector]
    public AudioSource source;
}
