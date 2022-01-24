using UnityEngine;

public class SpriteSlicer : BaseSlicer
{
    SpriteRenderer[] spriteRenderers;
    [SerializeField] Vector2 sizeOffset = Vector2.zero;

    protected override void Initialize()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void SetSize()
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.size = size + sizeOffset;
        }
    }
}
