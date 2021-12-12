using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] Vector2 size;
    [SerializeField] bool hasDoor;
    [Range(0, 1)]
    [SerializeField] float doorPosition;
    [SerializeField] float doorOffsetY;
    [SerializeField] float wallOffsetY;

    BoxCollider2D[] boxColliders;

    GameObject fullWall;
    GameObject wallWithDoor;
    Transform doorTransform;

    private void Awake()
    {
        Initialize();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += _OnValidate;
    }

    private void _OnValidate()
    {
        if (this == null || Application.isPlaying) return;
        Initialize();
            SetSize();
    }
#endif

    public void SetSize(float width, float height)
    {
        size.x = width;
        size.y = height;
        SetSize();
    }

    public void SetSize(Vector2 size)
    {
        this.size = size;
        SetSize();
    }

    void SetSize()
    {
        if (!hasDoor)
        {
            fullWall.SetActive(true);
            wallWithDoor.SetActive(false);
            boxColliders[0].size = size;
            boxColliders[0].offset = new Vector2(0, wallOffsetY);
            return;
        }
        fullWall.SetActive(false);
        wallWithDoor.SetActive(true);

        float doorWidth = boxColliders[3].size.x;
        float wallwidthMinusDoor = size.x - doorWidth;

        float width1 = wallwidthMinusDoor * doorPosition;
        float offset1 = size.x / 2 - width1 * .5f;

        float width2 = wallwidthMinusDoor * (1 - doorPosition);
        float offset2 = size.x / 2 - width2 * .5f;

        boxColliders[1].size = new Vector2(width1, size.y);
        boxColliders[1].offset = new Vector2(-offset1, wallOffsetY);

        boxColliders[2].size = new Vector2(width2, size.y);
        boxColliders[2].offset = new Vector2(offset2, wallOffsetY);

        doorTransform.localPosition = new Vector3((doorPosition - .5f) * wallwidthMinusDoor, doorOffsetY);
    }

    void Initialize()
    {
        fullWall.SetActive(true);
        wallWithDoor.SetActive(true);

        boxColliders = GetComponentsInChildren<BoxCollider2D>();
        fullWall = transform.GetChild(0).gameObject;
        wallWithDoor = transform.GetChild(1).gameObject;
        doorTransform = wallWithDoor.transform.GetChild(2);
    }
}
