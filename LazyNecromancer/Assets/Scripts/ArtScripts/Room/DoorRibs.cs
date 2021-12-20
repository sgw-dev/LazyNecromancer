using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRibs : MonoBehaviour
{
    Animator animator;
    DoorManager doorManager;
    public DoorManager DoorManager { get; set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Open", false);
        }
    }

    void OpenDoor() {
        if (!DoorManager.IsLocked)
        {
            animator.SetBool("Open", true);
        }
    }
}
