using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoidController : MonoBehaviour
{
    public GameObject enemyPrefab;
    [Range(0,100)]
    public int numberOfBoids;
    
    public float boidSpeed;
    public int boidDamage;
    [Range(0,10)]
    public int MaxAttacksPerSecond;

    public int Xmax;
    public int Xmin;
    public int Ymax;
    public int Ymin;

    List<EnemyBoid> boids;

    public float Separation;
    public float Cohesion;
    public float Alignment;

    private Vector3 target;

    public  float behaviourTimerMax;
    private float behaviourTimer;
    public  float swapvalue;
    private float swappedvalue;

    bool followMouse = true;

    float attacktimer;

    public Text atktimer;

    void Start() {
        attacktimer = 0f;
        behaviourTimer = 0f;
        CreateBoids();
    }

    public void CreateBoids() {
        boids = new List<EnemyBoid>();
        for(int i = 0; i < numberOfBoids; i++) {
            var tmp = GameObject.Instantiate(enemyPrefab);
            tmp.transform.position = RandomInBounds();
            var boidscript = tmp.GetComponent<EnemyBoid>();
            //set boid vars
            boidscript.speed = boidSpeed;
            boidscript.enemycon = this;
            boids.Add(boidscript);
        }
    }

    public Vector3 RandomInBounds() {
        return new Vector3(
            transform.position.x + Random.Range(Xmin,Xmax),
            transform.position.y + Random.Range(Ymin,Ymax),
            0
        );
    }

    void Update() {
        atktimer.text = ""+attacktimer;
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        BehaviourChange();
        MoveBoids();
        attacktimer+=Time.deltaTime;
    }

    void BehaviourChange() {
        behaviourTimer+=Time.deltaTime;
        if(behaviourTimer >= behaviourTimerMax) {
            behaviourTimer=0f;
            swappedvalue = Separation;
            Separation = swapvalue;
            swapvalue = swappedvalue;
        }
    }

    void OutOfBoundsBreak(EnemyBoid b) {
        if(b.transform.position.x > Xmax || b.transform.position.x < Xmin) {
                Debug.Break();
            }
            if(b.transform.position.y > Ymax || b.transform.position.y < Ymin) {
                Debug.Break();
        }
    }

    void MoveBoids() {
        foreach(EnemyBoid b in boids) {
            Vector3 v1 = Rule1(b);
            Vector3 v2 = Rule2(b);
            Vector3 v3 = Rule3(b);
            Vector3 bp = BoundPosition(b);
            Vector3 t = Target(b);
            b.velocity = b.velocity + v1 + v2 + v3 + bp + (followMouse?t:Vector3.zero);// + more stuff
            //b.velocity  = b.velocity + t;
            // debugger.text = "V1 = " + v1 + "\n" +
            //                 "V2 = " + v2 + "\n" +
            //                 "V3 = " + v3 + "\n" +
            //                 "bp = " + bp + "\n" +
            //                 "t  = " + t +"";
            if(b.velocity.magnitude > 5) {
                b.velocity= (b.velocity / b.velocity.magnitude) * boidSpeed;
            }

            //Debugging only
            // OutOfBoundsBreak(b);
            //Debugging only
        
        }
    }


    Vector3 Target(EnemyBoid boid) {
        Vector3 dir = target- boid.transform.position;
        dir.z=0f;
        return dir.normalized;
    }
    
    //cohesion
    Vector3 Rule1(EnemyBoid b) {
        return CenterOfMass(b);
    }

    //Separation
    Vector3 Rule2(EnemyBoid boid) {
        Vector3 c = Vector3.zero;
        foreach(EnemyBoid b in boids) {
            Vector3 mag = (b.transform.position - boid.transform.position);
            if(mag.magnitude < Separation) {
                c = c - mag.normalized;
            }
        }
        return c;
    }

    //Alignment
    Vector3 Rule3(EnemyBoid boid) {
        Vector3 percievedVelocity = Vector3.zero;
        
        foreach(EnemyBoid b in boids) {
            if(!boid.Equals(b)){
                percievedVelocity = percievedVelocity + b.velocity;
            }
        }

        percievedVelocity /= (boids.Count-1);

        return percievedVelocity.normalized;
    }


    //percieved center
    Vector3 CenterOfMass(EnemyBoid boid) {
        Vector3 mass = Vector3.zero;
        foreach(EnemyBoid b in boids) {
            if(boid.Equals(b)) {

            } else {
                mass += b.transform.position;
            }
        }
        Vector3 pc =  mass/(boids.Count-1);
        return ((pc-boid.transform.position)/100).normalized;//set to 100
    }

    Vector3 BoundPosition(EnemyBoid boid) {
        Vector3 v = Vector3.zero;

        if(boid.transform.position.x < Xmin) {
            v.x = 10f;
        } else if(boid.transform.position.x > Xmax) {
            v.x = -10f;
        } else if(boid.transform.position.y < Ymin) {
            v.y = 10f;
        } else if( boid.transform.position.y > Ymax) {
            v.y = -10f;
        }
        
        return v;
    }

    public void Damage<T>(GameObject obj) {
        // Debug.Log("Damage<"+typeof(T)+"> ::"+obj.name);
        if(attacktimer < (1.0f/MaxAttacksPerSecond)) {
            return;
        }
        
        attacktimer = 0f; //reset timer

        T healthscript = GetComponent<T>();

        if(healthscript != null) {
            //healthscript.takeDmg();
            Debug.Log(obj.name + " took " + boidDamage + ".");
        }
    }

    public void ToggleFollow() {
        followMouse = ! followMouse;
    }
}
