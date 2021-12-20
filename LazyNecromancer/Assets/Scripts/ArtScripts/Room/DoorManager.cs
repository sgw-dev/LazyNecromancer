using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    List<Animator> ribsDoorAnims;
    List<Animator> skullDoorAnims;

    bool locked;
    bool cleared;

    public bool IsLocked => locked;
    public bool IsCleared => cleared;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        ribsDoorAnims = new List<Animator>();
        skullDoorAnims = new List<Animator>();

        DoorLockTrap[] doorLockTraps = GetComponentsInChildren<DoorLockTrap>();
        foreach(DoorLockTrap dlt in doorLockTraps)
        {
            dlt.DoorManager = this;
        }

        DoorRibs[] doorRibs = GetComponentsInChildren<DoorRibs>();
        foreach (DoorRibs dr in doorRibs)
        {
            dr.DoorManager = this;
            Animator[] anims = dr.transform.parent.GetComponentsInChildren<Animator>(true);
            ribsDoorAnims.Add(anims[0]);
            skullDoorAnims.Add(anims[1]);
        }
        ResetRoom();
    }

    public void LockRoom()
    {
        if(cleared || locked) { return; }

        locked = true;

        foreach(Animator a in skullDoorAnims)
        {
            a.transform.parent.gameObject.SetActive(true);
        }
        SetDoorsAnim(skullDoorAnims, false);
        SetDoorsAnim(ribsDoorAnims, false);
    }

    public void RoomCleared()
    {
        cleared = true;
        locked = false;

        SetDoorsAnim(skullDoorAnims, true);
    }

    public void ResetRoom()
    {
        cleared = false;
        locked = false;
        SetDoorsAnim(skullDoorAnims, true);
        SetDoorsAnim(ribsDoorAnims, false);
    }

    void SetDoorsAnim(List<Animator> animators, bool isOpen)
    {
        foreach (Animator anim in animators)
        {
            anim.SetBool("Open", isOpen);
        }
    }
}
