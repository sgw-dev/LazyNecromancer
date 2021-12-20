using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TorchSlicer : BaseSlicer
{
    [SerializeField] GameObject torchPrefab;
    [SerializeField] Vector2 baseSize;
    [SerializeField] Vector2 padding;
    [SerializeField] Vector2 offset;

    List<Transform> torches = new List<Transform>();

    // Bug: The Prefab's are calling this and because they are Prefabs it kicks the object into the scene
    //      unparented. Thus the extra lights
    //  Solution 1: generate only in play mode
    //  Solution 2: Editor Button to generate them then save as prefab
    //  Solution 3: Use PrefabUtility.InstantiatePrefab, probly wont work but cool to know

    protected override void Initialize()
    {
        torches.Clear();
        transform.GetComponentsInDirectChildren<Transform>(torches, true);
    }

    protected override void SetSize()
    {
        Vector2 offsetSize = size + offset;
        List<Vector2> torchPositions = new List<Vector2>();
        int numHorizontal = (int)((offsetSize.x - padding.x) / baseSize.x / 2) + 1;
        int numVertical = (int)((offsetSize.y - padding.y) / baseSize.y / 2) + 1;

        // North/South
        for (int i = 0; i < numHorizontal; i++)
        {
            float x = i * baseSize.x + padding.x;
            float y = offsetSize.y / 2;
            torchPositions.Add(new Vector2(x, y));
            torchPositions.Add(new Vector2(-x, y));
            torchPositions.Add(new Vector2(x, -y));
            torchPositions.Add(new Vector2(-x, -y));
        }

        // East/West
        for (int i = 0; i < numVertical; i++)
        {
            float x = offsetSize.x / 2;
            float y = i * baseSize.y + padding.y;

            torchPositions.Add(new Vector2(x, y));
            torchPositions.Add(new Vector2(x, -y));
            torchPositions.Add(new Vector2(-x, y));
            torchPositions.Add(new Vector2(-x, -y));
        }

        int maxNumTorches = Mathf.Max(torchPositions.Count, torches.Count);
        for (int i = 0; i < maxNumTorches; i++)
        {
            if (i >= torchPositions.Count)
            {
                torches[torchPositions.Count].SafeDestroy();
                torches.RemoveAt(torchPositions.Count);
                continue;
            }
            else if (i >= torches.Count)
            {
                torches.Add(Instantiate(torchPrefab, this.transform).transform);
            }
            torches[i].localPosition = torchPositions[i];
        }
    }

    public void ResetTorches()
    {
        foreach (Transform t in torches)
        {
            t.SafeDestroy();
        }
        Initialize();
        SetSize();
    }
}

public class Torch
{
    ParticleSystem torchParticles;
    Light2D torchLight;

    public Torch(ParticleSystem particleSystem, Light2D light2D)
    {
        torchParticles = particleSystem;
        torchLight = light2D;
    }

    public Torch(GameObject gameObject)
    {
        torchParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        torchLight = gameObject.GetComponentInChildren<Light2D>();
    }

    public void On()
    {
        torchParticles.Play();
        torchLight.enabled = true;
    }

    public void Off()
    {
        torchParticles.Stop();
        torchLight.enabled = false;
    }
}