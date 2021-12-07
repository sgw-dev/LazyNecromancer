using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRibs : MonoBehaviour
{
    RoomManager2 roomManager;
    Animator animator;

    private void Awake()
    {
        roomManager = GetComponentInParent<RoomManager2>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        print("Collision Ribs");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        print("Triggered Ribs");
    }

    void OpenDoor() {
        if (!roomManager.isLocked())
        {
            animator.SetBool("Open", true);
        }
    }
}
