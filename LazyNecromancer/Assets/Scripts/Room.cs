using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LevelGen;

public class Room : MonoBehaviour
{
    private Dictionary<Direction, GameObject> doorList;
    public GameObject north;
    public GameObject east;
    public GameObject south;
    public GameObject west;

    public int roomId;

    public int depth;

    public Text debugText;
    private int hash = 0;
    public int UniqueHash { get { return hash; } set { hash = value; debugText.text = "" + hash; } }
    public Image nImage;
    public Image sImage;
    public Image eImage;
    public Image wImage;

    // Start is called before the first frame update
    void Start()
    {
        /*SpriteRenderer sp = gameObject.transform.GetChild(transform.childCount-1).GetComponent<SpriteRenderer>();
        IEnumerator temp = flash(sp);
        StartCoroutine(temp);*/
    }
    public void initialize(int prefabNumber)
    {
        RoomController rc = this.GetComponent<RoomController>();
        rc.CreateDoorsConverted(prefabNumber);
        doorList = new Dictionary<Direction, GameObject>();
        if (rc.HasDoor(Direction.NORTH))
        {
            doorList.Add(Direction.NORTH, North);
        }
        if (rc.HasDoor(Direction.SOUTH))
        {
            doorList.Add(Direction.SOUTH, South);
        }
        if (rc.HasDoor(Direction.EAST))
        {
            doorList.Add(Direction.EAST, East);
        }
        if (rc.HasDoor(Direction.WEST))
        {
            doorList.Add(Direction.WEST, West);
        }
    }
    public void initialize((int, int)element)
    {
        RoomController rc = this.GetComponent<RoomController>();
        rc.CreateDoors(element.Item1);
        doorList = new Dictionary<Direction, GameObject>();
        if (rc.HasDoor(Direction.NORTH))
        {
            doorList.Add(Direction.NORTH, North);
        }
        if (rc.HasDoor(Direction.SOUTH))
        {
            doorList.Add(Direction.SOUTH, South);
        }
        if (rc.HasDoor(Direction.EAST))
        {
            doorList.Add(Direction.EAST, East);
        }
        if (rc.HasDoor(Direction.WEST))
        {
            doorList.Add(Direction.WEST, West);
        }
    }
    public GameObject North
    {
        get
        {
            return north;
        }
        set
        {
            north = value;
            //nImage.color = LevelGen.setColor;
        }
    }
    public GameObject South
    {
        get
        {
            return south;
        }
        set
        {
            south = value;
            //sImage.color = LevelGen.setColor;
        }
    }
    public GameObject East
    {
        get
        {
            return east;
        }
        set
        {
            east = value;
            //eImage.color = LevelGen.setColor;
        }
    }
    public GameObject West
    {
        get
        {
            return west;
        }
        set
        {
            west = value;
            //wImage.color = LevelGen.setColor;
        }
    }
    public Dictionary<Direction, GameObject> DoorList
    {
        get
        {
            return doorList;
        }
    }

}
