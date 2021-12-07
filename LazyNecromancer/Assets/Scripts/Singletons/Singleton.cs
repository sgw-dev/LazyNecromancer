using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    public static T Instance
    {
        get { return m_Instance; }
    }

    public virtual void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = (T)FindObjectOfType(typeof(T));
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
