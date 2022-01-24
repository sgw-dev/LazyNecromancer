using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwaySlicer : BaseSlicer
{
    [Space(20)]
    [SerializeField] bool hasInnerMask;
    [SerializeField] bool hasOuterMask;

    SpriteSlicer[] spriteSlicers;
    Transform innerMask;
    Transform outerMask;

    protected override void Initialize()
    {
        spriteSlicers = GetComponentsInChildren<SpriteSlicer>(true);
        InitializeChildren(spriteSlicers);

        innerMask = transform.GetComponentInDirectChildren<Transform>(2, true);
        outerMask = transform.GetComponentInDirectChildren<Transform>(3, true);

        innerMask.gameObject.SetActive(hasInnerMask);
        outerMask.gameObject.SetActive(hasOuterMask);
    }

    protected override void SetSize()
    {
        foreach (SpriteSlicer ss in spriteSlicers)
        {
            ss.SetSize(size);
        }
        float maskPositionY = (size.y - 4) / 2;
        innerMask.localPosition = new Vector2(0, -maskPositionY);
        outerMask.localPosition = new Vector2(0, maskPositionY);
    }
}
