using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    RoomSlicer roomSlicer;

    int[] CONVERSION_TABLE =
    {
        0b0001,
        0b0010,
        0b0100,
        0b1000,
        0b0011,
        0b0110,
        0b0011,
        0b1001,
        0b0101,
        0b1010,
        0b0111,
        0b1110,
        0b1101,
        0b1011,
        0b1111,
        0b0000
    };

    private void Awake()
    {
        roomSlicer = GetComponent<RoomSlicer>();
    }

    public void CreateDoorsConverted(int indexOfSpencersPrefab)
    {
        CreateDoors(CONVERSION_TABLE[indexOfSpencersPrefab]);
    }

    public void CreateDoors(int doorIndex)
    {
        roomSlicer.SetDoorSettings(doorIndex);
        roomSlicer.ReInitialize();
    }

    public bool HasDoor(Direction direction)
    {
        return roomSlicer.DoorSettings[(int)direction].HasDoor;
    }
}
