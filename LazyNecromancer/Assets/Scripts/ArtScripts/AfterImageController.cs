using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageController : MonoBehaviour
{
    [Range(0, .25f)]
    [SerializeField] float frequency = .3f;
    [SerializeField] float afterImageDuration = .25f;
    [SerializeField] int maxNumAfterImages = 10;
    [SerializeField] Gradient gradient;

    ParticleSystem[] particleSystems;

    [SerializeField] GameObject parentObject;
    SpriteRenderer[] parentSprites;
    Transform[] parentSpriteTransforms;

    AfterImage[] afterImagePool;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        parentSprites = parentObject.GetComponentsInChildren<SpriteRenderer>();
        int numParentSprites = parentSprites.Length;

        parentSpriteTransforms = new Transform[numParentSprites];
        for (int i = 0; i < numParentSprites; i++)
        {
            parentSpriteTransforms[i] = parentSprites[i].transform;
        }

        afterImagePool = new AfterImage[maxNumAfterImages];

        Transform afterImageHolder = new GameObject(gameObject.name + " After Image Pool").transform;

        for (int i = 0; i < maxNumAfterImages; i++)
        {
            afterImagePool[i] = InitializeAfterImageGameObject(afterImageHolder, numParentSprites);
        }
    }

    AfterImage InitializeAfterImageGameObject(Transform aterImageHolder, int numParentSprites)
    {
        Transform parent = new GameObject("After Image").transform;
        AfterImage afterImage = new AfterImage(numParentSprites, parent);

        for (int i = 0; i < numParentSprites; i++)
        {
            GameObject go = new GameObject(parentSprites[i].gameObject.name + " After Image");
            go.transform.SetParent(parent.transform);
            afterImage.Add(i, go.AddComponent<SpriteRenderer>());
        }
        parent.SetParent(aterImageHolder);
        parent.gameObject.SetActive(false);
        return afterImage;
    }

    public void Play(float duration)
    {
        StartCoroutine(DisplayAfterImages(duration));
    }

    IEnumerator DisplayAfterImages(float duration)
    {
        PlayParticles();

        float endTime = Time.time + duration;
        int afterImageIndex = 0;

        while (Time.time < endTime)
        {
            if (afterImageIndex < maxNumAfterImages)
            {
                StartCoroutine(FadeAfterImage(afterImagePool[afterImageIndex]));
                afterImageIndex++;
            }
            yield return new WaitForSeconds(frequency);
        }

        StopParticles();
    }

    IEnumerator FadeAfterImage(AfterImage afterImage)
    {
        afterImage.Instantiate(transform.position, parentSprites, parentSpriteTransforms);
        float elapsedTime = 0;

        while (elapsedTime < afterImageDuration)
        {
            afterImage.ChangeSpriteColor(gradient.Evaluate(elapsedTime / afterImageDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        afterImage.Destroy();
    }

    void PlayParticles()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }

    void StopParticles()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Stop();
        }
    }
}
