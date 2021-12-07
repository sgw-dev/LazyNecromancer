using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage
{
    Transform[] spriteTransforms;
    SpriteRenderer[] spriteRenderers;
    Transform cachedTransform;

    public AfterImage (int length, Transform transform)
    {
        spriteRenderers = new SpriteRenderer[length];
        spriteTransforms = new Transform[length];
        cachedTransform = transform;
    }

    public void Add(int index, SpriteRenderer spriteRenderer)
    {
        spriteRenderers[index] = spriteRenderer;
        spriteTransforms[index] = spriteRenderer.transform;
    }

    public void Instantiate(Vector3 position, SpriteRenderer[] spriteRenderers, Transform[] spriteTransforms)
    {
        cachedTransform.gameObject.SetActive(true);
        cachedTransform.position = position;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (!spriteRenderers[i].enabled || !spriteRenderers[i].gameObject.activeInHierarchy)
            {
                this.spriteRenderers[i].enabled = false;
                continue;
            }

            // Mirror SpriteRenderer
            this.spriteRenderers[i].enabled = true;
            this.spriteRenderers[i].sprite = spriteRenderers[i].sprite;
            this.spriteRenderers[i].sortingOrder = spriteRenderers[i].sortingOrder;
            this.spriteRenderers[i].sortingLayerName = spriteRenderers[i].sortingLayerName;
            this.spriteRenderers[i].sortingLayerID = spriteRenderers[i].sortingLayerID;

            // Mirror Transform
            this.spriteTransforms[i].localPosition = spriteTransforms[i].localPosition;
            this.spriteTransforms[i].rotation = spriteTransforms[i].rotation;
            this.spriteTransforms[i].localScale = spriteTransforms[i].localScale;
        }
    }

    public void ChangeSpriteColor(Color color)
    {
        foreach( SpriteRenderer sr in spriteRenderers)
        {
            sr.color = color;
        }
    }

    public void Destroy()
    {
        cachedTransform.gameObject.SetActive(false);
    }
}
