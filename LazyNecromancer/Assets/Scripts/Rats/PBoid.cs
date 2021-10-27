using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBoid : MonoBehaviour {

    BoidSettings settings;
    PartionedBoidController manager;

    //State
    [HideInInspector] public Vector3 position, oldPosition;
    [HideInInspector] public Vector3 up, oldUp;
    Vector3 velocity;

    //Updated
    Vector3 acceleration;
    //[HideInInspector]
    public Vector3 avgFlockHeading;
    //[HideInInspector]
    public Vector3 avgAvoidanceHeading;
    //[HideInInspector]
    public Vector3 centreOfFlockmates;
    //[HideInInspector]
    public int numPerceivedFlockmates;

    //Cached
    Transform cachedTransform;
    Transform target;
    Rigidbody2D rb;
    
    public PBoid previousBoid, nextBoid;
    public Cell cell;

    public void Awake() {
        cachedTransform = transform;
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void Initialize(PartionedBoidController bm, BoidSettings settings, Transform target) {
        manager = bm;
        this.target = target;
        this.settings = settings;

        position = cachedTransform.position;
        up = cachedTransform.up;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.up * startSpeed;
    }

    public void UpdateBoid() {
        oldPosition = position;
        oldUp = up;
        acceleration = Vector3.zero;

                avgFlockHeading = new Vector3(cell.flockHeadingTotal.x - oldUp.x, cell.flockHeadingTotal.y - oldUp.y, 0);
        centreOfFlockmates = new Vector3(cell.flockCentreTotal.x - oldPosition.x, cell.flockCentreTotal.y - oldPosition.y, 0);
        numPerceivedFlockmates = cell.totalNumOfBoids - 1;

        
        if (target != null)
        {
            Vector3 offsetToTarget = new Vector3(target.position.x - position.x, target.position.y - position.y, 0);
            acceleration = SteerTowards(offsetToTarget);
            //acceleration = CalculateTargetVector();
            acceleration = new Vector3(acceleration.x * settings.targetWeight, acceleration.y * settings.targetWeight, 0);
        }
        

        if (numPerceivedFlockmates != 0)
        {
            centreOfFlockmates = new Vector3(centreOfFlockmates.x / numPerceivedFlockmates, centreOfFlockmates.y / numPerceivedFlockmates, 0);

            Vector3 offsetToFlockmatesCentre = new Vector3(centreOfFlockmates.x - position.x, centreOfFlockmates.y - position.y, 0);

            //DebugVectors(offsetToFlockmatesCentre);

            Vector3 alignmentForce = SteerTowards(avgFlockHeading);
            alignmentForce = new Vector3(alignmentForce.x * settings.alignWeight, alignmentForce.y * settings.alignWeight);

            Vector3 cohesionForce = SteerTowards(offsetToFlockmatesCentre);
            cohesionForce = new Vector3(cohesionForce.x * settings.cohesionWeight, cohesionForce.y * settings.cohesionWeight, 0);

            Vector3 seperationForce = SteerTowards(avgAvoidanceHeading);
            seperationForce = new Vector3(seperationForce.x * settings.seperateWeight, seperationForce.y * settings.seperateWeight, 0);

            acceleration = new Vector3(
                acceleration.x + alignmentForce.x + cohesionForce.x + seperationForce.x,
                acceleration.y + alignmentForce.y + cohesionForce.y + seperationForce.y,
                0);
        }

        if (IsHeadingForCollision())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir);
            collisionAvoidForce = new Vector3(collisionAvoidForce.x * settings.avoidCollisionWeight, collisionAvoidForce.y * settings.avoidCollisionWeight, 0);
            acceleration = new Vector3(collisionAvoidForce.x, collisionAvoidForce.y, 0);
        }

        float t = Time.deltaTime;
        velocity = new Vector3(acceleration.x * t + velocity.x, acceleration.y * t + velocity.y, 0);
        
        float speed = velocity.magnitude;
        Vector3 dir = new Vector3(velocity.x / speed, velocity.y / speed, 0);
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);

        velocity = new Vector3(dir.x * speed, dir.y * speed, 0);

        position = new Vector3(velocity.x * t + position.x, velocity.y * t + position.y, 0);
        rb.transform.up = dir;
        rb.transform.position = position;
        up = dir;
    }

    Vector3 SteerTowards(Vector3 vector) {
        vector = MyNormalize(vector);
        Vector3 v = new Vector3( vector.x * settings.maxSpeed - velocity.x , vector.y * settings.maxSpeed - velocity.y, 0);
        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }
    
    bool IsHeadingForCollision() {

        if (Physics2D.CircleCast(position, settings.boundsRadius, up, settings.collisionAvoidDst, settings.obstacleMask)) {
            return true;
        }
        return false;
    }

    Vector3 ObstacleRays() {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
            if (!Physics2D.CircleCast(position, settings.boundsRadius, dir, settings.collisionAvoidDst, settings.obstacleMask))
            {
                return dir;
            }
        }

        return up;
    }
    
    public void OnDestroy() {
        cell.RemoveBoid(this);
    }

    public void DebugVectors(Vector3 centre) {
        Debug.DrawRay(position, avgFlockHeading, Color.green);
        Debug.DrawRay(position, centre * .4f, Color.blue);
        Debug.DrawRay(position, avgAvoidanceHeading, Color.red);
    }    

    public Vector3 MyNormalize(Vector3 vector) {
        if(vector.x == 0 && vector.y == 0)
        {
            return new Vector3(0, 0, 0);
        }
        float m = vector.magnitude;
        return new Vector3(vector.x / m, vector.y / m, 0);
    }
    
}
public class BoidHelper {

    const int numViewDirections = 300 / 5;
    public static readonly Vector3[] directions;
    public static readonly Vector3[] offsets;

    static BoidHelper() {

        directions = new Vector3[BoidHelper.numViewDirections];

        float angleIncrement = 5;

        for (int i = 0; i < numViewDirections / angleIncrement; i++)
        {
            int n = ((i % 2 == 1) ? -1 : 1) * ((i + 1) / 2);
            directions[i] = Quaternion.Euler(0, 0, n * angleIncrement) * Vector2.up;
        }

        offsets = new Vector3[2000];
        for(int i = 0; i < offsets.Length; i++)
        {
            offsets[i] = (Vector3)Random.insideUnitCircle;
        }
    }

    public static Vector3 GetOffset() {
        return offsets[Random.Range(0, 2000)];
    }
}