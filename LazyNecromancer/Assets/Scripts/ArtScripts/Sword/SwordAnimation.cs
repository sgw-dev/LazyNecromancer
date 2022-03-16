using System.Collections;
using UnityEngine;

public class SwordAnimation : MonoBehaviour
{
    [SerializeField] SwordAnimationSettings settings;

    Vector3 pivotPoint = Vector3.zero;

    SpriteRenderer[] spriteRenderers;
    Collider2D[] collider2Ds;
    ParticleSystem[] particles;

    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles)
        {
            var main = ps.main;
            main.duration = settings.Duration;
            main.startLifetime = settings.Duration;
        }

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        collider2Ds = GetComponentsInChildren<Collider2D>();
        enableSprite(false);
        enableCollider(false);
    }

    public void TestAnimation()
    {
        transform.localPosition = pivotPoint;
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        StartCoroutine(_PlayAnimation());
    }

    IEnumerator _PlayAnimation()
    {
        foreach (ParticleSystem ps in particles)
        {
            ps.Play();
        }

        enableSprite(true);
        enableCollider(true);

        pivotPoint = transform.parent.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < settings.Duration)
        {
            float precent = elapsedTime / settings.Duration;

            setTransform(precent);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        setTransform(1);

        enableSprite(false);
        enableCollider(false);
    }

    void setTransform(float time)
    {
        float arc = settings.EvaluateArc(time);
        float radius = settings.EvaluateRadius(time);
        float rotation = settings.EvaluateRotation(time);

        Vector3 position = Vector3.zero;
        arc *= Mathf.Deg2Rad;

        position.x = Mathf.Cos(arc);
        position.y = Mathf.Sin(arc);
        transform.localPosition = pivotPoint + position * radius;
        transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    public void enableSprite(bool enable)
    {
        foreach (SpriteRenderer sp in spriteRenderers)
        {
            sp.enabled = enable;
        }
    }

    public void enableCollider(bool enable)
    {
        foreach (Collider2D c2d in collider2Ds)
        {
            c2d.enabled = enable;
        }
    }

    public SwordAnimationSettings Settings => settings;
}
