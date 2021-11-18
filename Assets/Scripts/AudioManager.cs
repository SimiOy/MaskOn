using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {

        if (instance == null) //sole instance of this class; can't be more than one, destroy otherwise
            instance = this;
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

            s.source.volume = s.volume;
            s.old_volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("BgMusic");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Check Spelling of sound Clip");
            return;
        }
        s.source.Play();
    }

    public void VolumeChange(string name, float newVolume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Check Spelling of sound Clip");
            return;
        }
        s.source.volume = newVolume;
    }

    public void SFX_volume(bool isSfx)
    {
        if (isSfx == false)
        {
            foreach (Sound s in sounds)
            {
                if (s.name != "BgMusic" && s.name != "SpecialEvent")
                    s.source.volume = 0f;
            }
        }
        else
            if (isSfx)
        {
            foreach (Sound s in sounds)
            {
                if (s.name != "BgMusic" && s.name != "SpecialEvent")
                {
                   // Debug.Log(s.old_volume);
                    s.source.volume = s.old_volume;
                }
            }
        }
    }

    public void PauseAllMusic()
    {
        foreach (Sound s in sounds)
        {
            s.source.Pause();
        }
    }

    public void PlayAllMusic()
    {
        Sound s = Array.Find(sounds, sound => sound.name == "BgMusic");
        if (s == null)
        {
            Debug.Log("Check Spelling of sound Clip");
            return;
        }
        s.source.Play();
    }
}
