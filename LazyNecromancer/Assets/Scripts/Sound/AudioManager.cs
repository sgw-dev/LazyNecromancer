using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] int audioSourcePoolSize;

    LinkedList<AudioSourceData> freePool;
    LinkedList<AudioSourceData> activePool;
    LinkedList<AudioSourceData> priorityPool;


    //Do this by Mixer group???

    /*Audio gruop requests to play
     * find open audio source
     * Add source to list and remove from free pool
     * Load audio group into source
     * play
     */
    private void Awake()
    {
        freePool = new LinkedList<AudioSourceData>();
        activePool = new LinkedList<AudioSourceData>();
        priorityPool = new LinkedList<AudioSourceData>();

        while(freePool.Count < audioSourcePoolSize)
        {
            GameObject go = new GameObject("AudioSource " + freePool.Count);
            go.transform.SetParent(transform);
            freePool.AddFirst(new AudioSourceData(go.AddComponent<AudioSource>()));
        }
    }

    public bool Play(AudioGroup audioGroup, ref LinkedListNode<AudioSourceData> node,  bool forcePlay = false, bool isInterruptable = true)
    {
        if(freePool.Count < 0)
        {
            if (!forcePlay)
            {
                return false;
            }
            OpenUpAudioSource(activePool.First);
        }

        node = freePool.First;
        freePool.Remove(node);
        node.Value.coroutine = PlayAudio(node);
        node.Value.Load(audioGroup);

        if (isInterruptable)
        {
            activePool.AddLast(node);
            return true;
        }
        priorityPool.AddLast(node);
        return true;
    }

    public void OpenUpAudioSource(LinkedListNode<AudioSourceData> node)
    {
        node.List.Remove(node);
        freePool.AddFirst(node);
    }

    IEnumerator PlayAudio(LinkedListNode<AudioSourceData> node)
    {
        AudioSourceData data = node.Value;
        foreach(Audio a in data.AudioGroup.audios)
        {
            data.Load(a);
            yield return new WaitWhile(() => data.audioSource.isPlaying);
        }
        OpenUpAudioSource(node);
    }
}

public class AudioSourceData
{
    public AudioSource audioSource;
    public IEnumerator coroutine;
    AudioGroup audioGroup;
    public AudioGroup AudioGroup => audioGroup;

    public AudioSourceData(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    public void Load(AudioGroup audioGroup)
    {
        this.audioGroup = audioGroup;
        audioSource.outputAudioMixerGroup = audioGroup.audioMixerGroup;
    }

    public void Load(Audio audio)
    {
        audioSource.clip = audio.audioClip;
        audioSource.volume = audio.maxVolume;
        audioSource.pitch = audio.pitch;
        audioSource.loop = audio.isLoop;
        audioSource.Play();
    }
}
