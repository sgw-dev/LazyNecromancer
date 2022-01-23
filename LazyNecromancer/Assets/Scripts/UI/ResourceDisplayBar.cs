using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplayBar : MonoBehaviour
{
    [SerializeField] ResourceIcon resourcePrefab;
    [Min(0)]
    [SerializeField] int maxNumIcons;
    [Min(0)]
    [SerializeField] int numIcons;
    [Min(0)]
    [SerializeField] float valuePerIcon;

    ResourceIcon[] resourceIcons;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        resourceIcons = new ResourceIcon[maxNumIcons];
        for(int i = 0; i < maxNumIcons; i++)
        {
            resourceIcons[i] = Instantiate(resourcePrefab, transform);
        }
        ActivateIcons();
    }

    public void UpdateValue(float value)
    {
        for (int i = 0; i < numIcons; i++)
        {
            resourceIcons[i].SetValue(value / valuePerIcon);
            value -= valuePerIcon;
        }
    }

    public void UpdateNumIcons(int numIcons)
    {
        this.numIcons = Mathf.Min(numIcons, maxNumIcons);
        ActivateIcons();
    }

    void ActivateIcons()
    {
        for (int i = 0; i < maxNumIcons; i++)
        {
            resourceIcons[i].gameObject.SetActive(i < numIcons);
        }
    }
}
