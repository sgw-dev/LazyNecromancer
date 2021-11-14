using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleRotator : MonoBehaviour
{
    [SerializeField] MagicCircleComponent[] components;
    SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        RotateComponents();
    }

    void RotateComponents()
    {
        foreach(MagicCircleComponent ele in components)
        {
            ele.Rotate();
        }
    }

    public void EnableSprites(bool enabled)
    {
        foreach(SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = enabled;
        }
    }

    public void CreateComponentList()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        components = new MagicCircleComponent[transforms.Length - 1];

        for (int i = 1; i < transforms.Length; i++)
        {
            components[i - 1] = new MagicCircleComponent(transforms[i]);
        }
    }
}

[System.Serializable]
public class MagicCircleComponent
{
    [SerializeField] Transform component;
    [SerializeField] float rotationRate;

    public MagicCircleComponent(Transform component)
    {
        this.component = component;
    }

    public void Rotate()
    {
        component.Rotate(0, 0, rotationRate * Time.deltaTime);
    }
}
