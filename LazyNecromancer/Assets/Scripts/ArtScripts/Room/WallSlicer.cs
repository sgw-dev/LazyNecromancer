using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlicer : BaseSlicer
{
    [Space(10)]
    [SerializeField] DoorSettings doorSettings;
    [SerializeField] Vector2 doorOffset = Vector2.zero;
    [Space(10)]
    [SerializeField] Vector2 wallOffset = Vector2.zero;

    Transform doorTransform;
    BoxCollider2D[] boxColliders;
    SpriteSlicer[] spriteSlicers;

    protected override void Initialize()
    {
        doorTransform = transform.GetComponentInDirectChildren<Transform>(2, true);
        if (doorTransform == null)
        {
            doorSettings = new DoorSettings(false, doorSettings.DoorPosition);
            Debug.LogWarning("Could Not Find Door: " + gameObject.name);
        }
        boxColliders = GetComponentsInChildren<BoxCollider2D>(true);
        spriteSlicers = transform.GetComponentsInDirectChildren<SpriteSlicer>(true);
        InitializeChildren(spriteSlicers);
    }

    protected override void SetSize()
    {
        foreach (SpriteSlicer ss in spriteSlicers)
        {
            ss.SetSize(size);
        }

        if (!doorSettings.HasDoor)
        {
            boxColliders[0].enabled = true;
            boxColliders[0].size = size;
            boxColliders[0].offset = wallOffset;
            boxColliders[1].enabled = false;
            if(doorTransform != null)
            {
                doorTransform.gameObject.SetActive(false);
            }
            return;
        }

        boxColliders[0].enabled = true;
        boxColliders[1].enabled = true;
        doorTransform.gameObject.SetActive(true);

        float doorWidth = boxColliders[2].size.x;
        float wallwidthMinusDoor = size.x - doorWidth;
        float halfSizeX = size.x / 2;

        float width1 = wallwidthMinusDoor * doorSettings.DoorPosition;
        float offsetX1 = halfSizeX - width1 * .5f;

        float width2 = wallwidthMinusDoor * (1 - doorSettings.DoorPosition);
        float offsetX2 = halfSizeX - width2 * .5f;

        boxColliders[0].size = new Vector2(width1, size.y);
        boxColliders[0].offset = new Vector2(-offsetX1, 0) + wallOffset;

        boxColliders[1].size = new Vector2(width2, size.y);
        boxColliders[1].offset = new Vector2(offsetX2, 0) + wallOffset;

        doorTransform.localPosition = new Vector2((doorSettings.DoorPosition - .5f) * wallwidthMinusDoor, 0) + doorOffset;
    }

    public DoorSettings DoorSettings
    {
        get { return doorSettings; }
        set
        {
            doorSettings = new DoorSettings(value.HasDoor, value.DoorPosition);
        }
    }
}

[System.Serializable]
public struct DoorSettings
{
    [SerializeField] bool hasDoor;
    [Range(0, 1)]
    [SerializeField] float doorPosition;

    public bool HasDoor => hasDoor;
    public float DoorPosition => doorPosition;

    public DoorSettings(bool hasDoor = false, float doorPosition = .5f)
    {
        this.hasDoor = hasDoor;
        this.doorPosition = doorPosition;
    }
}
