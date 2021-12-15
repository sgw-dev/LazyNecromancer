using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager2 : MonoBehaviour
{
    List<Animator> skullDoorAnims;
    List<Animator> ribsDoorAnims;

    bool locked;
    bool cleared;

    public void LockRoom()
    {
        locked = true;

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

    public bool isLocked()
    {
        return locked;
    }

    void SetDoorsAnim(List<Animator> animators, bool isOpen)
    {
        foreach (Animator anim in animators)
        {
            anim.SetBool("Open", isOpen);
        }
    }
}
