using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
public class AnimationControllerTester : MonoBehaviour
{
    PlayerAnimationController animationController;

    private void Awake()
    {
        animationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 directionalInput = new Vector2(x, y);
        animationController.ResetState();
        animationController.InputDirection(directionalInput);
        animationController.UpdateAnimation();
    }
}
