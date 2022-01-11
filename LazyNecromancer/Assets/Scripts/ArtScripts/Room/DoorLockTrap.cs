using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockTrap : MonoBehaviour
{
    DoorManager doorManager;
    public DoorManager DoorManager { get; set; }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DoorManager.LockRoom();
        }
    }

}
