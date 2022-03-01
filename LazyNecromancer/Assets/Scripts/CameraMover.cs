using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float speed;
    private Vector3 target;
    public Vector3 Target
    {
        set
        {
            if(value.x != target.x || value.y != target.y)
            {
                target = value;
            }
        }
    }
    public void Start()
    {
        target = Vector3.zero;
        Debug.Log("Entered Start of Camera Mover");
    }

    public void Update()
    {
        if(this.transform.position.x != target.x || this.transform.position.y != target.y)
        {
            float newX = Mathf.MoveTowards(this.transform.position.x, target.x, speed * Time.fixedDeltaTime);
            float newY = Mathf.MoveTowards(this.transform.position.y, target.y, speed * Time.fixedDeltaTime);
            this.transform.position = new Vector3(newX, newY, -10);
            Debug.Log("Camera Moving...");
        }
    }
}
