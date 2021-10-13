using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    public List<GameObject> roomObjects; // The prefabs
    public List<Room> rooms; // The scripts attached to each prefab
    public GameObject head;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in roomObjects)
        {
            rooms.Add(obj.GetComponent<Room>());
        }
        /*head = generateLevel();
        GameObject headObject = Instantiate(roomObjects[head.RoomScript.roomId]);
        displayLevel(headObject, head);*/
        GameObject headObject = Instantiate(roomObjects[0]);
        levelGen2(headObject, 3);
    }
    void levelGen2(GameObject head, int depth)
    {
        if(depth > 0)
        {
            Room room = head.GetComponent<Room>();
            foreach(string key in room.DoorList.Keys)
            {
                int newDepth = depth - 1;
                switch (key)
                {
                    case "North":
                        if(room.north == null)
                        {
                            room.north = Instantiate(roomObjects[Random.Range(0, 4)]);
                            room.north.transform.parent = room.transform;
                            room.north.transform.localPosition = new Vector3(0, 20, 0);
                            room.north.GetComponent<Room>().south = room.north;
                            levelGen2(room.north, newDepth);
                        }
                        break;
                    case "South":
                        if (room.south == null)
                        {
                            room.south = Instantiate(roomObjects[Random.Range(0, 4)]);
                            room.south.transform.parent = room.transform;
                            room.south.transform.localPosition = new Vector3(0, -20, 0);
                            room.south.GetComponent<Room>().north = room.south;
                            levelGen2(room.south, newDepth);
                        }
                        break;
                    case "East":
                        if (room.east == null)
                        {
                            room.east = Instantiate(roomObjects[Random.Range(0, 4)]);
                            room.east.transform.parent = room.transform;
                            room.east.transform.localPosition = new Vector3(20, 0, 0);
                            room.east.GetComponent<Room>().west = room.east;
                            levelGen2(room.east, newDepth);
                        }
                        break;
                    case "West":
                        if (room.west == null)
                        {
                            room.west = Instantiate(roomObjects[Random.Range(0, 4)]);
                            room.west.transform.parent = room.transform;
                            room.west.transform.localPosition = new Vector3(-20, 0, 0);
                            room.west.GetComponent<Room>().east = room.west;
                            levelGen2(room.west, newDepth);
                        }
                        break;
                }
            }
        }
    }
    Node generateLevel()
    {
        Node headnode = new Node((Room)rooms[0], null);
        generate(headnode, 3);
        return headnode;
    }
    void generate(Node head, int depth)
    {
        for(int i = 0; i < head.Children.Length; i++)
        {
            head.Children[i] = new Node((Room)rooms[Random.Range(0, 4)], head);
            if(depth > 0)
            {
                generate(head.Children[i], --depth);
            }
            
        }
    }
    void displayLevel(GameObject headObject, Node headNode)
    {
        foreach(Node child in headNode.Children)
        {
            if(child != null)
            {
                GameObject childObject = Instantiate(roomObjects[headNode.RoomScript.roomId], headObject.transform);
                displayLevel(childObject, child);
            }
            
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
