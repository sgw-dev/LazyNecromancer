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
    public GameObject North
    {
        get
        {
            return north;
        }
        set
        {
            north = value;
            nImage.color = LevelGen.setColor;
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
            sImage.color = LevelGen.setColor;
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
            eImage.color = LevelGen.setColor;
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
            wImage.color = LevelGen.setColor;
        }
    }
    public Dictionary<Direction, GameObject> DoorList
    {
        get
        {
            if(doorList == null)
            {
                doorList = new Dictionary<Direction, GameObject>();
                Transform temp = transform.Find("NorthDoor");
                if (temp != null)
                {
                    doorList.Add(Direction.NORTH, West);
                }
                temp = transform.Find("SouthDoor");
                if (temp != null)
                {
                    doorList.Add(Direction.SOUTH, South);
                }
                temp = transform.Find("EastDoor");
                if (temp != null)
                {
                    doorList.Add(Direction.EAST, East);
                }
                temp = transform.Find("WestDoor");
                if (temp != null)
                {
                    doorList.Add(Direction.WEST, West);
                }
            }
            return doorList;
        }
    }

}
