using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    protected Animator[] animators;
    protected SpriteRenderer[] spriteRenderers;

    protected Vector2 inputDirection = Vector2.down;
    protected bool idling;
    protected bool walking;

    protected virtual void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public virtual void InputDirection(Vector2 direction)
    {
        walking = direction.sqrMagnitude > .001f;
        if(walking)
        {
            inputDirection = direction;
        }
    }

    public virtual void ResetState()
    {
        walking = false;
        idling = false;
    }

    public virtual void UpdateAnimation()
    {
        foreach(Animator anim in animators)
        {
            anim.SetFloat("Horizontal", inputDirection.x);
            anim.SetFloat("Vertical", inputDirection.y);
            anim.SetBool("Idling", idling);
            anim.SetBool("Walking", walking);
        }
    }
}
