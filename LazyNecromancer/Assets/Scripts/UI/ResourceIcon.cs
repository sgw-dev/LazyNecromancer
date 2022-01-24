using UnityEngine;

public class ResourceIcon : MonoBehaviour
{
    RectTransform resource;

    private void Awake()
    {
        resource = transform.GetComponentInDirectChildren<RectTransform>(2, true);
    }

    public void SetValue(float precentage)
    {
        precentage = Mathf.Clamp01(precentage);
        resource.localScale = new Vector2(precentage, precentage);
    }
}
