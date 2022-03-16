using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    protected MusicManager() { }

    AudioSource[] sources;
    int currentSource;

    public override void Awake()
    {
        base.Awake();
        sources = new AudioSource[2];
        sources[0] = gameObject.AddComponent<AudioSource>();
        sources[1] = gameObject.AddComponent<AudioSource>();
        currentSource = 0;
    }

    void PlayMusic(Audio audio)
    {

    }
}
