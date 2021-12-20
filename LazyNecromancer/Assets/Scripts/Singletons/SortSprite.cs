using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortSprite : MonoBehaviour
{
    enum SortType
    {
        None,
        OnlyAtStart,
        EveryUpdate
    }

    [SerializeField] SortType sortType;
    [SerializeField] int sortOffset;

    void Start()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        switch (sortType)
        {
            case SortType.EveryUpdate:
                SpriteSortManager.Instance.AddToUpdatedSprites(sprites, transform, sortOffset);
                break;
            case SortType.OnlyAtStart:
                SpriteSortManager.Instance.UpdateSpriteSortOrder(sprites, transform.position.y, sortOffset);
                break;
        }

    }
}
