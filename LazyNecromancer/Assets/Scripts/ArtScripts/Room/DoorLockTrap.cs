using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockTrap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collision Skull");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Triggered Skull");
    }
}
