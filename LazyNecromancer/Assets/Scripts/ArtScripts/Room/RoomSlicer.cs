using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSlicer : BaseSlicer
{
    [Space(10)]
    [SerializeField] Vector2 floorOffset = Vector2.zero;
    [SerializeField] Vector2 torchOffset = Vector2.zero;
    [Space(20)]
    [SerializeField] DoorSettings[] doorSettings;

    WallSlicer[] wallSlicers;
    SpriteSlicer[] spriteSlicers;
    TorchManager torchManager;

    public void ReInitialize()
    {
        Initialize();
        SetSize();
    }

    protected override void Initialize()
    {
        wallSlicers = GetComponentsInChildren<WallSlicer>(true);
        InitializeChildren(wallSlicers);

        for (int i = 0; i < doorSettings.Length; i++)
        {
            wallSlicers[i].DoorSettings = doorSettings[i];
        }

        spriteSlicers = transform.GetComponentsInDirectChildren<SpriteSlicer>(true);
        InitializeChildren(spriteSlicers);

        torchManager = GetComponentInChildren<TorchManager>();
    }

    protected override void SetSize()
    {
        Vector2 sizeReducedNS = new Vector2(size.x - 1, size.y - 1);
        Vector2 sizeReducedEW = new Vector2(sizeReducedNS.y, sizeReducedNS.x);

        //North
        wallSlicers[0].SetSize(sizeReducedNS);
        wallSlicers[0].transform.localPosition = new Vector3(0, size.y / 2 - 1.5f);

        //East
        wallSlicers[1].SetSize(sizeReducedEW);
        wallSlicers[1].transform.localPosition = new Vector3(size.x / 2, 0);

        //South
        wallSlicers[2].SetSize(sizeReducedNS);
        wallSlicers[2].transform.localPosition = new Vector3(0, -size.y / 2);

        //West
        wallSlicers[3].SetSize(sizeReducedEW);
        wallSlicers[3].transform.localPosition = new Vector3(-size.x / 2, 0);

        spriteSlicers[0].transform.localPosition = floorOffset;
        foreach (SpriteSlicer ss in spriteSlicers)
        {
            ss.SetSize(size);
        }

        if (torchManager != null)
        {
            torchManager.transform.localPosition = torchOffset;
            torchManager.ResetTorches(new Vector2(size.x - 1, size.y - 2));
        }
    }

    public void SetDoorSettings(int doorIndex)
    {
        for( int i = 0; i < doorSettings.Length; i++)
        {
            doorSettings[i] = new DoorSettings((doorIndex >> i & 1) == 1);
        }
    }

    public DoorSettings[] DoorSettings => doorSettings;
}
