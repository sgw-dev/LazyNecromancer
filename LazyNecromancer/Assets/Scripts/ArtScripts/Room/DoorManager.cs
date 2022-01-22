using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    List<Animator> ribsDoorAnims;
    List<Animator> skullDoorAnims;

    TorchManager2 torchManager;

    bool locked;
    bool cleared;
    public bool InDoorWay { get; set; }
    bool isHead;

    public bool IsHead {
        get { return isHead; }
        set { isHead = value; cleared = true; }
    }

    public bool IsLocked => locked;
    public bool IsCleared => cleared;

    private void Awake()
    {
        Initialize();
        torchManager = FindObjectOfType<TorchManager2>();
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

        LightSwitch[] lightSwitches = GetComponentsInChildren<LightSwitch>();
        foreach (LightSwitch ls in lightSwitches)
        {
            ls.DoorManager = this;
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
        if(cleared || locked || InDoorWay) { return; }

        locked = true;

        foreach(Animator a in skullDoorAnims)
        {
            a.transform.parent.gameObject.SetActive(true);
        }
        SetDoorsAnim(skullDoorAnims, false);
        SetDoorsAnim(ribsDoorAnims, false);
        
        this.GetComponent<Spawner>().PlayerEntersRoom();
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

    public void TurnOnLights(Direction direction)
    {
        if (InDoorWay) { return; }
        torchManager.RequestTorches(transform.position, direction, !IsCleared);
    }

    void SetDoorsAnim(List<Animator> animators, bool isOpen)
    {
        foreach (Animator anim in animators)
        {
            anim.SetBool("Open", isOpen);
        }
    }
}
