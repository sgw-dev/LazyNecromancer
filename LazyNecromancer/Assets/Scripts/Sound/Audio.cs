using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Audio
{
    public AudioClip audioClip;
    [Range(0, 1)] public float maxVolume = 1;
    [Range(.1f, 3)] public float pitch = 1;
    public float delay;
    public bool isLoop;
}

[CreateAssetMenu(fileName = "new Audio", menuName = "Audio")]
public class AudioGroup : ScriptableObject
{
    public Audio[] audios;
    public AudioPlayType audioPlayType = AudioPlayType.InOrder;
    public AudioMixerGroup audioMixerGroup;
}

public enum AudioPlayType
{
    InOrder,
    Random,
    Parallel
}
