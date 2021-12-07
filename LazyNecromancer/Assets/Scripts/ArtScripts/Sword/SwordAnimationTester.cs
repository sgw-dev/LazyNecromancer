using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimationTester : MonoBehaviour
{
    [SerializeField] SwordAnimation swordAnimation;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swordAnimation.TestAnimation();
        }
    }
}
