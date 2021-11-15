using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    public GameObject player;
    public float maxRange;
    public float minRange;

    public float stepSize;

    public float attackSpeed;

    private List<DirectionValue> moveDirections;

    public GameObject Room { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        moveDirections = new List<DirectionValue>();
        //Vector3 forward = player.transform.position - gameObject.transform.position;
        //forward = forward.normalized;
        //gameObject.GetComponent<Rigidbody2D>().velocity = forward;
        //gameObject.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(90, Vector3.forward) * forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        moveDirections.Clear();
        // Make a list of 16 possible move directions
        Vector3 forward = player.transform.position - gameObject.transform.position;
        forward = forward.normalized;
        moveDirections.Add(new DirectionValue(forward, 0));
        //for(float i = 11.25f; i<360; i+= 11.25f)
        for (float i = 22.5f; i < 360; i += 22.5f)
        {
            moveDirections.Add(new DirectionValue(Quaternion.AngleAxis(i, Vector3.up) * forward, 0));
        }
        DirectionValue smallest = null;
        // Evaluate those directions
        foreach (DirectionValue dv in moveDirections)
        {
            // Each vector is normalized, so multiply it by stepSize
            Vector3 newPos = dv.Dir * stepSize;
            // Add the new move vector to the current position to get the new position
            newPos = newPos + gameObject.transform.position;
            // Set the value to be the distance to the player from the new position
            dv.Value = Vector3.Distance(newPos, player.transform.position);
            dv.Dir = newPos;

            // Save the smallest value
            if(smallest == null || dv.Value < smallest.Value)
            {
                smallest = dv;
            }
        }

        // Pick Smallest Direction
        gameObject.transform.position = smallest.Dir;





        /*
        Vector3 playerPos = player.transform.position;
        Vector3 selfPos = gameObject.transform.position;

        Vector3 playerDirection = playerPos - selfPos;
        float playerDistance = Vector3.Distance(playerPos, selfPos);
        Vector3 direction;
        if (playerDistance > maxRange)
        {
            direction = playerDirection.normalized * moveSpeed;
        }else if(playerDistance < minRange)
        {
            direction = playerDirection.normalized * -moveSpeed;
        }
        else
        {
            direction = Vector3.zero;
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = direction;*/
    }
    private class DirectionValue
    {
        public Vector3 Dir { get; set; }
        public float Value { get; set; }
        public DirectionValue(Vector3 d, float v)
        {
            Dir = d;
            Value = v;
        }
    }
}
