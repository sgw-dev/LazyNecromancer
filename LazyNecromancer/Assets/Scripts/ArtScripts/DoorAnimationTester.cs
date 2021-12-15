using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationTester : MonoBehaviour
{
    public bool open;
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        animator.SetBool("Open", open);
    }
}
