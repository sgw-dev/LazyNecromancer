using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplayTester : MonoBehaviour
{
    [Min(0)]
    [SerializeField] float value;
    [Range(0, 10)]
    [SerializeField] int numIcons;
    ResourceDisplayBar resourceDisplayBar;

    private void Start()
    {
        resourceDisplayBar = GetComponent<ResourceDisplayBar>();
    }

    private void Update()
    {
        resourceDisplayBar.UpdateNumIcons(numIcons);
        resourceDisplayBar.UpdateValue(value);
    }
}
