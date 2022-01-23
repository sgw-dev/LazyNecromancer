using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lighter : MonoBehaviour
{
    Light2D light;
    ParticleSystem particles;

    [Range(0, .3f)]
    [SerializeField] float fluctuation = .1f;
    [SerializeField] float warmUpTime = .5f;
    float maxLightInnerRadius;
    float lightInnerRadius;
    float maxLightOuterRadius;
    float lightOuterRadius;

    bool isOn;

    private void Awake()
    {
        light = GetComponentInChildren<Light2D>();
        light.gameObject.SetActive(false);
        particles = GetComponentInChildren<ParticleSystem>();
        particles.Stop();

        maxLightInnerRadius = light.pointLightInnerRadius;
        maxLightOuterRadius = light.pointLightOuterRadius;
    }

    public void PlayNew()
    {
        isOn = true;
        light.gameObject.SetActive(true);
        particles.Play();

        lightInnerRadius = 0;
        lightOuterRadius = 0;
        StopAllCoroutines();
        StartCoroutine(Flicker());
        StartCoroutine(PlayAnimation(maxLightInnerRadius, maxLightOuterRadius));
    }

    public void Play()
    {
        isOn = true;
        light.gameObject.SetActive(true);
        particles.Play();

        lightInnerRadius = maxLightInnerRadius;
        lightOuterRadius = maxLightOuterRadius;
        StopAllCoroutines();
        StartCoroutine(Flicker());
    }

    public void Stop()
    {
        if (!isOn)
        {
            return;
        }
        isOn = false;
        particles.Stop();

        StopAllCoroutines();
        StartCoroutine(PlayAnimation(0, 0, true));
    }

    IEnumerator PlayAnimation(float toInner, float toOuter, bool turnOff = false)
    {
        float elapsedTime = 0;
        while (elapsedTime < warmUpTime)
        {
            float precent = elapsedTime / warmUpTime;
            lightInnerRadius = Mathf.Lerp(toInner, lightInnerRadius, precent);
            lightOuterRadius = Mathf.Lerp(toOuter, lightOuterRadius, precent);

            light.pointLightInnerRadius = lightInnerRadius;
            light.pointLightOuterRadius = lightOuterRadius;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (turnOff)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Flicker()
    {
        while (isOn)
        {
            light.pointLightInnerRadius = lightInnerRadius * (1 + Random.Range(-fluctuation, fluctuation));
            light.pointLightOuterRadius = lightOuterRadius * (1 + Random.Range(-fluctuation, fluctuation));
            yield return new WaitForSeconds(.1f);
        }
    }
}
