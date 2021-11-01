using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceManager : MonoBehaviour
{
    SpriteRenderer[] spriteRenderers;

    [SerializeField] Vector2 size;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += _OnValidate;
    }

    private void _OnValidate()
    {
        if (this == null) return;
        if (!Application.isPlaying)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            SetSize();
        }
    }
    #endif

    public void SetSize(float width, float height)
    {
        size.x = width;
        size.y = height;
        SetSize();
    }

    public void SetSize(Vector2 size)
    {
        this.size = size;
        SetSize();
    }

    void SetSize()
    {
        foreach(SpriteRenderer sr in spriteRenderers)
        {
            sr.size = size;
        }
    }
}
