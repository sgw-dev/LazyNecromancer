using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] doors;
    private Dictionary<string, GameObject> doorList;
    public GameObject north;
    public GameObject east;
    public GameObject south;
    public GameObject west;

    
    public int roomId;
    // Start is called before the first frame update
    void Start()
    {
        /*doorList = new Dictionary<string, GameObject>();
        Transform temp = transform.Find("NorthDoor");
        if(temp != null)
        {
            doorList.Add("North", north);
        }
        temp = transform.Find("SouthDoor");
        if (temp != null)
        {
            doorList.Add("South", south);
        }
        temp = transform.Find("EastDoor");
        if (temp != null)
        {
            doorList.Add("East", east);
        }
        temp = transform.Find("WestDoor");
        if (temp != null)
        {
            doorList.Add("West", west);
        }*/
    }
    public Dictionary<string, GameObject> DoorList
    {
        get
        {
            if(doorList == null)
            {
                doorList = new Dictionary<string, GameObject>();
                Transform temp = transform.Find("NorthDoor");
                if (temp != null)
                {
                    doorList.Add("North", north);
                }
                temp = transform.Find("SouthDoor");
                if (temp != null)
                {
                    doorList.Add("South", south);
                }
                temp = transform.Find("EastDoor");
                if (temp != null)
                {
                    doorList.Add("East", east);
                }
                temp = transform.Find("WestDoor");
                if (temp != null)
                {
                    doorList.Add("West", west);
                }
            }
            return doorList;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
