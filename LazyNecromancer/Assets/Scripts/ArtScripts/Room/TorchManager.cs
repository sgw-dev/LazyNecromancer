using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchManager : MonoBehaviour
{
    [SerializeField] GameObject torchPrefab;
    [SerializeField] Vector2 size;
    [SerializeField] Vector2 torchSpacing;
    [SerializeField] Vector2 padding;
    [SerializeField] Vector2 offset;

    public void ResetTorches()
    {
        Initialize();
        SetSize();
    }

    public void ResetTorches(Vector2 size)
    {
        this.size = size;
        Initialize();
        SetSize();
    }

    protected void Initialize()
    {
        List<Transform> torches = new List<Transform>();
        transform.GetComponentsInDirectChildren<Transform>(torches, true);
        foreach (Transform t in torches)
        {
            t.SafeDestroy();
        }
        torches.Clear();
    }

    protected void SetSize()
    {
        Vector2 offsetSize = size;
        List<Vector2> torchPositions = new List<Vector2>();
        int numHorizontal = (int)((offsetSize.x - padding.x) / torchSpacing.x / 2) + 1;
        int numVertical = (int)((offsetSize.y - padding.y) / torchSpacing.y / 2) + 1;

        // North/South
        for (int i = 0; i < numHorizontal; i++)
        {
            float x = i * torchSpacing.x + padding.x;
            float y = offsetSize.y / 2;
            torchPositions.Add(new Vector2(x + offset.x, y));   // North
            torchPositions.Add(new Vector2(-x + offset.x, y));  // North
            torchPositions.Add(new Vector2(x + offset.x, -y));  // South
            torchPositions.Add(new Vector2(-x + offset.x, -y)); // South
        }

        // East/West
        for (int i = 0; i < numVertical; i++)
        {
            float x = offsetSize.x / 2;
            float y = i * torchSpacing.y + padding.y;

            torchPositions.Add(new Vector2(x, y + offset.y));   // West
            torchPositions.Add(new Vector2(x, -y + offset.y));  // West
            torchPositions.Add(new Vector2(-x, y + offset.y));  // East
            torchPositions.Add(new Vector2(-x, -y + offset.y)); // East
        }

        for (int i = 0; i < torchPositions.Count; i++)
        {
            Transform temp = Instantiate(torchPrefab, this.transform).transform;
            temp.localPosition = torchPositions[i];
        }
    }
}
