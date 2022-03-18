using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class BossPlacement {

    public static GameObject FindRoom(ConcurrentDictionary<(float, float), GameObject> allRooms) {
        
        GameObject bestFit = null;
        int maxDepth = int.MinValue;
        string debugmsg = "";
        foreach((float,float) key in allRooms.Keys) {
            // Debug.Log(allRooms[key].gameObject.name);
            Room r = allRooms[key].GetComponent<Room>();//
            int doorCount = r.DoorList.Count;

            debugmsg += allRooms[key].name + "\n";
            debugmsg += "doors = "+ doorCount + "\n";
            debugmsg += "d = " + r.depth + "\n";
            if(doorCount == 1 && r.depth > maxDepth) {
                maxDepth = r.depth;
                bestFit = allRooms[key];
            }
        }
        Debug.Log(debugmsg);
        return bestFit;
    }
}