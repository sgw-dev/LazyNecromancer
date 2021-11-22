using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleSpell : BaseSpell
{
    ParticleSystem[] particleSystems;
    MagicCircleRotator magicCircle;

    bool followTarget = true;

    protected override void Awake()
    {
        base.Awake();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        magicCircle = GetComponent<MagicCircleRotator>();
    }

    private void Start()
    {
        StartAttackTimer();
    }

    private void Update()
    {
        if (followTarget && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Play(mousePos);
        }
        else
        {
            followTarget = false;
        }
        SetVelocity();
    }

    public override void SetVelocity()
    {
        Vector3 distance = (target - transform.position);
        if(speed * speed > distance.sqrMagnitude)
        {
            rb.velocity = distance;
            return;
        }
        Vector2 normal = distance.normalized;
        rb.velocity = normal * speed;
    }

    protected override void StartDelayedSelfDestruct()
    {
        foreach (Collider2D ele in collider2Ds)
        {
            ele.enabled = false;
        }
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Stop();
        }
        magicCircle.EnableSprites(false);
        rb.velocity = Vector2.zero;
        followTarget = false;
        base.StartDelayedSelfDestruct();
    }
}
