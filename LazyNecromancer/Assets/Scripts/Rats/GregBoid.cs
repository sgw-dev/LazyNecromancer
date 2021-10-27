using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GregBoid : MonoBehaviour
{
    public Vector3 velocity;
    public float speed;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * velocity * Time.deltaTime,Space.World);
        //transform.LookAt((velocity + transform.position,transform.forward);
        Transform c = transform.GetChild(0);//.LookAt(Vector3.zero,transform.up);
        //c.LookAt(Vector3.zero,c.position+c.up);
        //c.LookAt(velocity,c.up);
        //c.Rotate(0,90f,0);
    }
}
