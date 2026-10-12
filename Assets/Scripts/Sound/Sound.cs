using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound // Class for all sounds
{
    public AudioClip clip; // Actual audio clip

    [Range (0f, 1f)]
    public float volume; // Volume of audio clip

    public string name; // name of audioclip

    [HideInInspector] 
    public AudioSource source; //source of audio
}
