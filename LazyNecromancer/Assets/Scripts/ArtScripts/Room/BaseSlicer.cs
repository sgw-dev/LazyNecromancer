using UnityEngine;

public abstract class BaseSlicer : MonoBehaviour
{
    [SerializeField] protected Vector2 size;
    [SerializeField] bool lockXSize;
    [SerializeField] bool lockYSize;

#if UNITY_EDITOR
    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += _OnValidate;
    }

    protected void _OnValidate()
    {
        if (this == null || Application.isPlaying) return;
        Initialize();
        SetSize();
    }
#endif
    
    public void SetSize(Vector2 size)
    {
        SetSize(size.x, size.y);
    }

    public void SetSize(float width, float height)
    {
        size.x = lockXSize ? size.x : width;
        size.y = lockYSize ? size.y : height;
        SetSize();
    }

    protected abstract void SetSize();

    protected abstract void Initialize();

    protected void InitializeChildren()
    {
        BaseSlicer[] slicers = transform.GetComponentsInDirectChildren<BaseSlicer>();
        InitializeChildren(slicers);
    }

    protected void InitializeChildren(BaseSlicer[] slicers)
    {
        foreach (BaseSlicer bs in slicers)
        {
            bs.Initialize();
        }
    }
}
