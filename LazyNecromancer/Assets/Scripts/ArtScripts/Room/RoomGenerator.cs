using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] Vector2 size;

    SliceManager nWallSliceManager;
    SliceManager floorSliceManager;

    SpriteRenderer wallBorderSpriteRenderer;

    Transform nWallTransform;

    private void Awake()
    {
        Initialize();
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
            Initialize();
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
        nWallSliceManager.SetSize(size.x - 1, 2);
        nWallTransform.localPosition = new Vector3(0, size.y / 2 - 1.5f);

        floorSliceManager.SetSize(size.x - 1, size.y - 3);

        wallBorderSpriteRenderer.size = size;
    }

    void Initialize()
    {
        SliceManager[] sliceManagers = GetComponentsInChildren<SliceManager>();

        nWallSliceManager = sliceManagers[0];
        nWallTransform = nWallSliceManager.transform;

        floorSliceManager = sliceManagers[1];
        floorSliceManager.transform.localPosition = new Vector3(0, -1, 0);

        wallBorderSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        wallBorderSpriteRenderer.transform.localPosition = Vector3.zero;
    }
        
}
