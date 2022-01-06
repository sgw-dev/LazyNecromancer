using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direction 
{ 
    NORTH = 0, 
    EAST = 1, 
    SOUTH = 2, 
    WEST = 3 
};
public static class Extenions
{
    public static Direction Invert(this Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH:
                return Direction.SOUTH;
            case Direction.SOUTH:
                return Direction.NORTH;
            case Direction.EAST:
                return Direction.WEST;
            case Direction.WEST:
                return Direction.EAST;
        }
        return Direction.NORTH;
    }
}
public class LevelGen : MonoBehaviour
{
    
    public GameObject roomSlice;
    public GameObject roomContainer;
    private GameObject headObject;

    public bool done = true;

    private IEnumerator routine;
    private ConcurrentDictionary<(float, float), GameObject> allRooms;

    private int roomCounter = 0;
    public bool waitFlag = false;

    public Color color;

    public float roomSize;
    public float roomSpawnDelay;

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

    [SerializeField]
    public static Color setColor;
    // Start is called before the first frame update
    void Start()
    {

        LevelGen.setColor = color;
        allRooms = new ConcurrentDictionary<(float, float), GameObject>();

        headObject = Instantiate(roomSlice, roomContainer.transform);
        headObject.GetComponent<Room>().initialize(0);
        headObject.GetComponent<DoorManager>().IsHead = true;
        allRooms.TryAdd((headObject.transform.position.x, headObject.transform.position.y), headObject);
        routine = levelGen2(headObject, 0);
        StartCoroutine(routine);
    }
    public void Reload()
    {
        roomCounter = 0;
        foreach (Transform child in roomContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        allRooms = new ConcurrentDictionary<(float, float), GameObject>();
        headObject = Instantiate(roomSlice, roomContainer.transform);
        headObject.GetComponent<Room>().initialize(0);
        headObject.GetComponent<DoorManager>().IsHead = true;
        allRooms.TryAdd((headObject.transform.position.x, headObject.transform.position.y), headObject);
        routine = levelGen2(headObject, 0);
        StartCoroutine(routine);
        done = true;
    }
    IEnumerator levelGen2(GameObject head, int depth)
    {
        
        Room room = head.GetComponent<Room>();
        foreach (Direction key in room.DoorList.Keys)
        {
            int newDepth = depth + 1;
            /*
            (float, float) roomPos = (room.transform.position.x, room.transform.position.y + 20f);
            switch (key)
            {
                case Direction.NORTH:
                    roomPos = (room.transform.position.x, room.transform.position.y + 20f);
                    break;
                case Direction.SOUTH:
                    roomPos = (room.transform.position.x, room.transform.position.y - 20f);
                    break;
                case Direction.EAST:
                    roomPos = (room.transform.position.x + 20f, room.transform.position.y);
                    break;
                case Direction.WEST:
                    roomPos = (room.transform.position.x - 20f, room.transform.position.y);
                    break;
            }
            yield return new WaitForSeconds(1f);
            roomCounter++;
            chooseRoomV3(key, newDepth, roomPos, roomCounter);
            room.DoorList[key].transform.parent = room.transform;
            room.DoorList[key].transform.localPosition = new Vector3(0, 20, 0);
            room.DoorList[key].GetComponent<Room>().UniqueHash = roomCounter;

            allRooms.TryAdd(roomPos, room.DoorList[key]);
            IEnumerator temp = levelGen2(room.DoorList[key], newDepth);
            yield return StartCoroutine(temp);
            */
            
            switch (key)
            {
                case Direction.NORTH:
                    if(room.North == null)
                    {
                        yield return new WaitForSeconds(roomSpawnDelay);
                        roomCounter++;
                        (float, float) roomPos = (room.transform.position.x, room.transform.position.y + roomSize);
                        chooseRoomV3(Direction.NORTH, newDepth, roomPos, roomCounter);
                        room.North.transform.localPosition = room.transform.position + new Vector3(0, roomSize, 0);
                        //room.North.GetComponent<Room>().UniqueHash = roomCounter;
                        
                        allRooms.TryAdd(roomPos, room.North);
                        IEnumerator temp = levelGen2(room.North, newDepth);
                        yield return StartCoroutine(temp);
                    }
                    break;
                case Direction.SOUTH:
                    if (room.South == null)
                    {
                        yield return new WaitForSeconds(roomSpawnDelay);
                        roomCounter++;
                        (float, float) roomPos = (room.transform.position.x, room.transform.position.y - roomSize);
                        chooseRoomV3(Direction.SOUTH, newDepth, roomPos, roomCounter);
                        room.South.transform.localPosition = room.transform.position + new Vector3(0, -roomSize, 0);
                        //room.South.GetComponent<Room>().UniqueHash = roomCounter;
                        
                        allRooms.TryAdd(roomPos, room.South);
                        IEnumerator temp = levelGen2(room.South, newDepth);
                        yield return StartCoroutine(temp);
                    }
                    break;
                case Direction.EAST:
                    if (room.East == null)
                    {
                        yield return new WaitForSeconds(roomSpawnDelay);
                        roomCounter++;
                        (float, float) roomPos = (room.transform.position.x + roomSize, room.transform.position.y);
                        chooseRoomV3(Direction.EAST, newDepth, roomPos, roomCounter);
                        room.East.transform.localPosition = room.transform.position + new Vector3(roomSize, 0, 0);
                        //room.East.GetComponent<Room>().UniqueHash = roomCounter;
                        
                        allRooms.TryAdd((room.East.transform.position.x, room.East.transform.position.y), room.East);
                        IEnumerator temp = levelGen2(room.East, newDepth);
                        yield return StartCoroutine(temp);
                    }
                    break;
                case Direction.WEST:
                    if (room.West == null)
                    {
                        yield return new WaitForSeconds(roomSpawnDelay);
                        roomCounter++;
                        (float, float) roomPos = (room.transform.position.x - roomSize, room.transform.position.y);
                        chooseRoomV3(Direction.WEST, newDepth, roomPos, roomCounter);
                        room.West.transform.localPosition = room.transform.position + new Vector3(-roomSize, 0, 0);
                        //room.West.GetComponent<Room>().UniqueHash = roomCounter;
                        
                        allRooms.TryAdd(roomPos, room.West);
                        IEnumerator temp = levelGen2(room.West, newDepth);
                        yield return StartCoroutine(temp);
                    }
                    break;
            }
        }
        done = true;
    }
    /*
     * roomPos is position of the room you are generating
     */
    GameObject chooseRoomV3(Direction dir, int depth, (float x, float y) roomPos, int roomNumber)
    {
        // These directions are always relative to the new room
        Dictionary<Direction, Room> needsDoorsHere = new Dictionary<Direction, Room>();
        List<Direction> cannotHaveDoorHere = new List<Direction>();
        // Check all 4 directions for neighbors
        foreach (Direction side in System.Enum.GetValues(typeof(Direction)))
        {
            (float x, float y) newPos = (roomPos.x, roomPos.y + roomSize);
            switch (side)
            {
                case Direction.NORTH:
                    newPos = (roomPos.x, roomPos.y + roomSize);
                    break;
                case Direction.SOUTH:
                    newPos = (roomPos.x, roomPos.y - roomSize);
                    break;
                case Direction.EAST:
                    newPos = (roomPos.x + roomSize, roomPos.y);
                    break;
                case Direction.WEST:
                    newPos = (roomPos.x - roomSize, roomPos.y);
                    break;
            }
            if (allRooms.ContainsKey(newPos))
            {
                
                Room room = allRooms[newPos].GetComponent<Room>();
                // If that room has a door facing the new room (South)
                if (room.DoorList.ContainsKey(side.Invert()))
                {
                    // Add the starting direction (north) to the must have list
                    needsDoorsHere.Add(side, room);
                }
                else
                {
                    // There is a room here, but no door. So the new room cannot have a door here either
                    cannotHaveDoorHere.Add(side);
                }

            }
        }
        // We now have a list of sides that we need a door in, and a list of ones that cannot have a door
        int minDoors = 4;
        int maxDoors = 1;
        // Go through each possible door combination
        List<(int, int)> valid = new List<(int, int)>();
        foreach(int doorCombo in CONVERSION_TABLE)
        {
            bool validRoom = true;
            List<Direction> doorList = new List<Direction>();
            if(IsBitSet(doorCombo, 0))
            {
                doorList.Add(Direction.NORTH);
            }
            if (IsBitSet(doorCombo, 1))
            {
                doorList.Add(Direction.EAST);
            }
            if (IsBitSet(doorCombo, 2))
            {
                doorList.Add(Direction.SOUTH);
            }
            if (IsBitSet(doorCombo, 3))
            {
                doorList.Add(Direction.WEST);
            }
            // make sure there are doors where there should be
            foreach(Direction key in needsDoorsHere.Keys)
            {
                // if the room does not have the proper door, it is not valid
                if (!doorList.Contains(key))
                {
                    validRoom = false;
                }
            }
            // Make sure there are not doors where there shouldn't be
            foreach (Direction key in cannotHaveDoorHere)
            {
                // if the room does have a door here, it is not valid
                if (doorList.Contains(key))
                {
                    validRoom = false;
                }
            }
            // If the room is valid
            if (validRoom)
            {
                // record the max or min number of doors
                int numDoors = doorList.Count;
                maxDoors = (numDoors > maxDoors) ? numDoors : maxDoors;
                minDoors = (numDoors < minDoors) ? numDoors : minDoors;

                // Add it to the list
                valid.Add((doorCombo, numDoors));
            }
        }
        // the valid list now contains only rooms that are valid to place
        // Chose a random door based on how many doors you want
        int doors = chooseNumDoors(minDoors, maxDoors, depth);
        // Filter the valid list to only contain rooms with the desired number of doors
        valid = valid.FindAll(element => element.Item2 == doors);
        // Choose a random element from that list
        int index = Random.Range(0, valid.Count);
        GameObject choice = Instantiate(roomSlice, roomContainer.transform);
        
        //choice.GetComponent<Room>().UniqueHash = roomNumber;
        choice.GetComponent<Room>().initialize(valid[index]);
        choice.GetComponent<Spawner>().StartSpawning();
        choice.name = choice.name + ":" + roomNumber;
        // Assign parents to the room's needed doors
        foreach (Direction key in needsDoorsHere.Keys)
        {
            // Go to the room to the north and set it's south door to be the new room
            switch (key)
            {
                case Direction.NORTH:
                    needsDoorsHere[key].South = choice;
                    choice.GetComponent<Room>().North = needsDoorsHere[key].gameObject;
                    break;
                case Direction.SOUTH:
                    needsDoorsHere[key].North = choice;
                    choice.GetComponent<Room>().South = needsDoorsHere[key].gameObject;
                    break;
                case Direction.EAST:
                    needsDoorsHere[key].West = choice;
                    choice.GetComponent<Room>().East = needsDoorsHere[key].gameObject;
                    break;
                case Direction.WEST:
                    needsDoorsHere[key].East = choice;
                    choice.GetComponent<Room>().West = needsDoorsHere[key].gameObject;
                    break;
            }
        }
        return choice;
    }
    bool IsBitSet(int b, int pos)
    {
        return (b >> pos & 1) == 1;
    }
    int chooseNumDoors(int minDoors, int maxDoors, int depth)
    {
        // Limit depth to 10
        if(depth == 10)
        {
            return minDoors;
        }else if(depth == 1)
        {
            return maxDoors;
        }
        // min is 10, max is 40
        int minAdjusted = minDoors * 10;
        int maxAdjusted = maxDoors * 10;
        int random = Random.Range(minAdjusted, maxAdjusted);
        // before depth 5, encourage growth
        if(depth < 5)
        {
            random = random + (11 - depth);
        }
        else
        {
            // Encourage decay
            random = random - (5 + depth);
        }
        random = Mathf.Clamp(random, minAdjusted, maxAdjusted);
        return (random / 10);

    }
}
