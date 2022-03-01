using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private Vector3 start;
    private Vector3 end;
    private Vector3 offest = new Vector3(0, 0, -10);

    private bool moveFlag = false;
    private float count;

    public GameObject minimapCover;
    public GameObject player;
    private CameraMover CM;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        //minimapCover = this.transform.Find("Canvas/MinimapCover").gameObject;
        //minimapCover = this.transform.GetComponentInDirectChildren<Transform>(6, true).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        CM = Camera.main.GetComponent<CameraMover>();
        minX = this.transform.position.x - 10f;
        maxX = this.transform.position.x + 10f;
        minY = this.transform.position.y - 12f;
        maxY = this.transform.position.y + 8f;
        Debug.Log("RoomManager Start");
    }

    void Update()
    {
        
        if(player.transform.position.x >= minX && player.transform.position.x < maxX)
        {
            if (player.transform.position.y >= minY && player.transform.position.y < maxY)
            {
                CM.Target = this.transform.position;
                Debug.Log("Moved Target");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (minimapCover.activeSelf)
            {
                minimapCover.SetActive(false);
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (minimapCover.activeSelf)
            {
                minimapCover.SetActive(false);
            }
        }
    }
}
