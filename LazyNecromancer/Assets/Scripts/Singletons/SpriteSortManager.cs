using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortManager : Singleton<SpriteSortManager>
{
    [SerializeField] float sortScale;

    protected SpriteSortManager() { }

    List<SpriteTransform> updatedSprites = new List<SpriteTransform>();

    public override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        foreach (SpriteTransform st in updatedSprites)
        {
            UpdateSpriteSortOrder(st.spriteRenderers, st.spriteTransform.position.y, st.orderOffset);
        }
    }

    public void UpdateSpriteSortOrder(SpriteRenderer[] spriteRenderers, float y, int orderOffset = 0)
    {
        int order = (int)(y / sortScale) + orderOffset;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sortingOrder = order;
        }
    }

    public void AddToUpdatedSprites(SpriteRenderer[] spriteRenderers, Transform spriteTransform, int orderOffset = 0)
    {
        UpdateSpriteSortOrder(spriteRenderers, spriteTransform.position.y, orderOffset);
        updatedSprites.Add(
            new SpriteTransform(spriteRenderers, spriteTransform, orderOffset)
        );
    }

    struct SpriteTransform
    {
        public SpriteRenderer[] spriteRenderers;
        public Transform spriteTransform;
        public int orderOffset;

        public SpriteTransform(SpriteRenderer[] spriteRenderers, Transform spriteTransform, int orderOffset)
        {
            this.spriteRenderers = spriteRenderers;
            this.spriteTransform = spriteTransform;
            this.orderOffset = orderOffset;
        }
    }
}
