using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpell : BaseSpell
{
    ParticleSystem ps;
    BoxCollider2D boxCollider;
    SpriteRenderer border;

    [SerializeField] Vector2 colliderStartingSize;
    [SerializeField] Vector2 colliderEndingSize;

    protected override void Awake()
    {
        base.Awake();
        ps = GetComponentInChildren<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider2D>();
        border = GetComponentInChildren<SpriteRenderer>();

        ps.Stop();
        var main = ps.main;
        main.duration = attackDuration;
        ps.Play();
    }

    public override void Play()
    {
        RotateToTarget();
        StartAttackTimer();
        StartCoroutine(ScaleCollider());
        base.Play();
    }

    protected override void StartDelayedSelfDestruct()
    {
        foreach (Collider2D ele in collider2Ds)
        {
            ele.enabled = false;
        }
        border.enabled = false;
        rb.velocity = Vector2.zero;
        base.StartDelayedSelfDestruct();
    }

    IEnumerator ScaleCollider()
    {
        float elapsedTime = 0;

        while (elapsedTime < attackDuration)
        {
            Vector2 size = Vector2.Lerp(colliderStartingSize, colliderEndingSize, elapsedTime / attackDuration);
            boxCollider.size = size;
            border.size = size;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
