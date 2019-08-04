using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SFX : MonoBehaviour
{
    public AudioSource source;
    public Settings settings;
    public Sounds sounds;

    private void PlayRandom(AudioClip[] clips)
    {
        source.PlayOneShot(clips[Random.Range(0, clips.Length)], settings.GetGameVolume());
    }

    public void PlayButtonClick()
    {
        PlayRandom(sounds.ButtonClick);
    }
}
