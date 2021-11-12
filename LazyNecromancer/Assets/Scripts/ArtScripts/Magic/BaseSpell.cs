using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpell : MonoBehaviour
{
    protected List<BaseSpell> chainedSpells = new List<BaseSpell>();

    [SerializeField] protected Vector3 offset;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected float selfDestructDelay;

    protected Vector3 target;

    protected Rigidbody2D rb;
    protected Collider2D[] collider2Ds;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2Ds = GetComponentsInChildren<Collider2D>();
        transform.position += offset;
    }

    public virtual void Play(Vector3 target)
    {
        SetTarget(target);
        Play();
    }

    public virtual void Play()
    {
        rb.velocity = transform.right * speed;
    }

    protected virtual void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    protected virtual void RotateToTarget()
    {
        Vector3 dir = target - transform.position;
        float angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected virtual void SetSelfDestructAtTarget()
    {
        attackDuration = Vector3.Distance(transform.position, target) / speed;
        StartAttackTimer();
    }

    protected void StartAttackTimer()
    {
        StopCoroutine(AttackTimer());
        StartCoroutine(AttackTimer());
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(attackDuration);
        if(selfDestructDelay > 0)
        {
            StartDelayedSelfDestruct();
        }
    }

    protected virtual void StartDelayedSelfDestruct()
    {
        StopCoroutine(DelayedSelfDestruct());
        StartCoroutine(DelayedSelfDestruct());
    }

    IEnumerator DelayedSelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructDelay);
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if(chainedSpells.Count < 0)
        {
            return;
        }

        foreach(BaseSpell bs in chainedSpells)
        {
            BaseSpell temp = Instantiate(bs, transform.position, transform.rotation);
            temp.Play();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered");
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided");
    }

}
