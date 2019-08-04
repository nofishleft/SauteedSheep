using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 57)]
public class Settings : ScriptableObject
{
    [Range(0,1)]
    public float MasterVolume = 0.5f;

    [Range(0, 1)]
    public float MusicVolume = 0.5f;

    [Range(0, 1)]
    public float GameVolume = 0.5f;

    public float GetMusicVolume()
    {
        return MasterVolume * MusicVolume;
    }

    public float GetGameVolume()
    {
        return MasterVolume * GameVolume;
    }
}
