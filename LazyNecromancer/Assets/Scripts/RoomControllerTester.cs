using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControllerTester : MonoBehaviour
{
    [Range(0, 15)]
    [SerializeField] int doorIndex;
    int oldDoorIndex;
    [SerializeField] bool testWhichDoors;
    [SerializeField] bool testCrateDoorConverted;

    RoomController roomController;

    private void Awake()
    {
        roomController = GetComponent<RoomController>();
    }

    private void Update()
    {
        if(doorIndex != oldDoorIndex)
        {
            oldDoorIndex = doorIndex;
            if (testCrateDoorConverted)
            {
                roomController.CreateDoorsConverted(oldDoorIndex);
            }
            else
            {
                roomController.CreateDoors(oldDoorIndex);
            }
        }
        if (testWhichDoors)
        {
            PrintHasDoors();
            testWhichDoors = false;
        }
    }

    void PrintHasDoors()
    {
        Debug.Log("North = " + roomController.HasDoor(Direction.NORTH));
        Debug.Log("East = " + roomController.HasDoor(Direction.EAST));
        Debug.Log("West = " + roomController.HasDoor(Direction.WEST));
        Debug.Log("South = " + roomController.HasDoor(Direction.SOUTH));
    }
}
