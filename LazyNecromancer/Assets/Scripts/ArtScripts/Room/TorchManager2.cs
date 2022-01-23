using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchManager2 : MonoBehaviour
{
    [SerializeField] Lighter lighterPrefab;

    [SerializeField] int torchPoolSize;
    Lighter[] torches;
    int torchIndex;

    [SerializeField] Vector2 size;
    [SerializeField] Vector2 positionOffset;
    [SerializeField] Vector2 torchSpacing;
    [SerializeField] Vector2 padding;
    [SerializeField] Vector2 offset;

    public Vector2 Size { get { return size; } set { size = value; } }
    public Vector2 PositionOffset { get { return positionOffset; } set { positionOffset = value; } }
    public Vector2 TorchSpacing { get { return torchSpacing; } set { torchSpacing = value; } }
    public Vector2 Padding { get { return padding; } set { padding = value; } }
    public Vector2 Offset { get { return offset; } set { offset = value; } }

    private void Start()
    {
        torches = new Lighter[torchPoolSize];

        for (int i = 0; i < torchPoolSize; i++)
        {
            Lighter temp = Instantiate(lighterPrefab, transform);
            temp.gameObject.SetActive(false);
            torches[i] = temp;
        }
    }

    public void RequestTorches(Vector2 position, Direction direction, bool newRoom = false)
    {
        StopAllCoroutines();
        StartCoroutine(SpawnTorches(GenerateWallTorchePositions(direction), position + positionOffset, newRoom));
    }

    IEnumerator SpawnTorches(List<Vector2> torchPositions, Vector2 position, bool newRoom)
    {
        int numTorchPositions = torchPositions.Count;
        for (int i = 0; i < torchPoolSize; i++)
        {
            torchIndex = (torchIndex + 1) % torchPoolSize;
            if (i >= numTorchPositions)
            {
                torches[torchIndex].Stop();
                continue;
            }
            torches[torchIndex].gameObject.SetActive(true);
            torches[torchIndex].transform.position = torchPositions[i] + position;
            if (newRoom)
            {
                yield return new WaitForSeconds(.1f);
                torches[torchIndex].PlayNew();
                continue;
            }
            torches[torchIndex].Play();
        }
    }

    List<Vector2> GenerateWallTorchePositions(Direction direction)
    {
        Vector2 wallPos = new Vector2(Size.x / 2, Size.y / 2);
        int numHorizontalHalf = (int)((Size.x - Padding.x) / TorchSpacing.x / 2) + 1;
        int numVerticalHalf = (int)((Size.y - Padding.y) / TorchSpacing.y / 2) + 1;
        int numHorizontal = numHorizontalHalf * 2;
        int numVertical = numVerticalHalf * 2;


        Vector2[] northTorches = new Vector2[numHorizontal];
        Vector2[] southTorches = new Vector2[numHorizontal];
        Vector2[] westTorches = new Vector2[numVertical];
        Vector2[] eastTorches = new Vector2[numVertical];

        // North/South
        for (int i = 0; i < numHorizontalHalf; i++)
        {
            float x = i * TorchSpacing.x + Padding.x;

            northTorches[numHorizontalHalf - i - 1] = new Vector2(x + Offset.x, wallPos.y);
            northTorches[numHorizontalHalf + i] = new Vector2(-x + Offset.x, wallPos.y);

            southTorches[numHorizontalHalf - i - 1] = new Vector2(x + Offset.x, -wallPos.y);
            southTorches[numHorizontalHalf + i] = new Vector2(-x + Offset.x, -wallPos.y);
        }

        // East/West
        for (int i = 0; i < numVerticalHalf; i++)
        {
            float y = i * TorchSpacing.y + Padding.y;

            eastTorches[numVerticalHalf - i - 1] = new Vector2(wallPos.x, y + Offset.y);
            eastTorches[numVerticalHalf + i] = new Vector2(wallPos.x, -y + Offset.y);

            westTorches[numVerticalHalf - i - 1] = new Vector2(-wallPos.x, y + Offset.y);
            westTorches[numVerticalHalf + i] = new Vector2(-wallPos.x, -y + Offset.y);
        }

        switch(direction)
        {
            case Direction.NORTH:
                return MergeTorchArray(northTorches, eastTorches, westTorches, southTorches);
            case Direction.SOUTH:
                return MergeTorchArray(southTorches, westTorches, eastTorches, northTorches, true);
            case Direction.EAST:
                return MergeTorchArray(eastTorches, southTorches, northTorches, westTorches);
            default:
                return MergeTorchArray(westTorches, northTorches, southTorches, eastTorches, true);
        }
    }

    List<Vector2> MergeTorchArray(Vector2[] first, Vector2[] second, Vector2[] third, Vector2[] fourth, bool reverse = false)
    {
        List<Vector2> torchPositions = new List<Vector2>();
        int lengthFirstHalf = first.Length / 2;
        for (int i = 0; i < lengthFirstHalf; i++)
        {
            torchPositions.Add(first[lengthFirstHalf - i - 1]);
            torchPositions.Add(first[lengthFirstHalf + i]);
        }

        for (int i = 0; i < second.Length; i++)
        {
            int index = reverse ? second.Length - i - 1 : i;
            torchPositions.Add(second[index]);
            torchPositions.Add(third[index]);
        }

        for (int i = 0; i < lengthFirstHalf; i++)
        {
            torchPositions.Add(fourth[i]);
            torchPositions.Add(fourth[fourth.Length - i - 1]);
        }

        return torchPositions;
    }
}
