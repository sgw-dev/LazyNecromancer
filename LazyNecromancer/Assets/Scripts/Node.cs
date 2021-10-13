using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    private Room room;
    private GameObject roomObject;
    public Node[] Children { get; set; }
    public Node Parent { get; set; }

    public Node(Room rm, Node parent)
    {
        //RoomObject = obj;
        room = rm;
        Parent = parent;
        int numChildren = (parent == null) ? room.doors.Length : room.doors.Length - 1;
        Children = new Node[numChildren];
        
    }
    
    /*public GameObject RoomObject
    {
        set
        {
            roomObject = value;
            room = roomObject.GetComponent<Room>();
        }
        get
        {
            return roomObject;
        }
    }*/
    public Room RoomScript
    {
        get
        {
            return room;
        }
    }
    
}
