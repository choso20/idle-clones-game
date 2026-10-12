using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; //Array that'll hold all audio clips

    // Initialization method (before Start)
    void Awake()
    {
        foreach (Sound s in sounds) // For each sound in sound array, makes a corrosponding audio source with the array's variables
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void Play(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // Finds sound in sounds array where sound name is equal to name parameter

        if (name == "bgNoise") //loops if sound is bgNoise
        {
            s.source.loop = true;
        }

        if (name == "buildingSound")
        {
            if (s.source.isPlaying)
            {
                Sound s2 = Array.Find(sounds, sound => sound.name == "buildingSound2");

                if (s2.source.isPlaying)
                {
                    Sound s3 = Array.Find(sounds, sound => sound.name == "buildingSound3");

                    if (s3.source.isPlaying)
                    {
                        Sound s4 = Array.Find(sounds, sound => sound.name == "buildingSound4");

                        s4.source.volume = volume;
                        s4.source.Play();
                    }
                    else
                    {
                        s3.source.volume = volume;
                        s3.source.Play();
                    }
                }
                else
                {
                    s2.source.volume = volume;
                    s2.source.Play();
                }
            }
            else
            {
                s.source.volume = volume;
                s.source.Play();
            }
        }
        else
        {
            s.source.volume = volume; // sets volume to parameter volume
            s.source.Play(); // play sound
        }


    }

    public void bgVolChange() //Changes the volume of bgNoise
    {
        float percentage = ((float)GameManager.Instance.roomsMade / 8);

        Sound s = Array.Find(sounds, sound => sound.name == "bgNoise");

        s.source.volume = percentage;
    }

    public void Start() // Plays and sets volume of background noise to percentage of rooms currently made to max rooms
    {
        float percentage = ((float)GameManager.Instance.roomsMade / 6);
        
        Play("bgNoise", percentage);
    }
}
