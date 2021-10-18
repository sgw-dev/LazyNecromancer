using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
class Boid {
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 rule1;
    public Vector3 rule2;
    public Vector3 rule3;
}
*/
public class BoidController : MonoBehaviour {
    
    public int numberOfBoids;
    public int Xmax;
    public int Xmin;
    public int Ymax;
    public int Ymin;


    
    public float boidSpeed;
    ///////////
    public Sprite boidprefab;
    
    List<Boid> boids;
    static readonly float FACTOR = 100;
    public float Separation = 3;
    
    public void CreateBoids() {
        boids = new List<Boid>();
        for(int i = 0 ; i < numberOfBoids ; i++ ){
            //create boid
            //Boid b = new Boid();
            var temp = new GameObject();
            temp.transform.position = new Vector3(Random.Range(-20,20),Random.Range(-20,20),0);
            var spriteobj  = new GameObject();
            SpriteRenderer sprite = spriteobj.AddComponent<SpriteRenderer>();
            spriteobj.transform.parent = temp.transform;
            sprite.sprite = boidprefab;

            Boid b = temp.AddComponent<Boid>();
            b.speed = boidSpeed;
            boids.Add(b);
        }
    }

    void Start() {
        CreateBoids();
        //Debug.Break();
    }

    int counter = 0;
    void Update() {
        MoveBoids();
        if(counter > 100) {
            //Debug.Break();
            counter = 0;
        }
        counter ++;
    }

    void MoveBoids() {
        foreach(Boid b in boids) {
            Vector3 v1 = Rule1(b);
            Vector3 v2 = Rule2(b);
            Vector3 v3 = Rule3(b);
            Vector3 bp = BoundPosition(b);
            b.velocity = b.velocity + v1 + v2 +v3 + bp;// + more stuff
            if(b.velocity.magnitude > 5) {
                b.velocity= (b.velocity / b.velocity.magnitude) * boidSpeed;
            }
            //b.velocity = b.velocity.normalized;
            //b.position = b.position + b.velocity
        }
    }

    //cohesion
    Vector3 Rule1(Boid b) {
        return CenterOfMass(b);
    }
    
    //Separation
    Vector3 Rule2(Boid boid) {
        Vector3 c = Vector3.zero;
        foreach(Boid b in boids) {
            Vector3 mag = (b.transform.position - boid.transform.position);
            if(mag.magnitude < Separation) {
                c = c - mag.normalized;
            }
        }
        return c;
    }

    //Alignment
    Vector3 Rule3(Boid boid) {
        Vector3 percievedVelocity = Vector3.zero;
        
        foreach(Boid b in boids) {
            if(!boid.Equals(b)){
                percievedVelocity = percievedVelocity + b.velocity;
            }
        }

        percievedVelocity /= (boids.Count-1);

        return percievedVelocity.normalized;
    }

    //percieved center
    Vector3 CenterOfMass(Boid boid) {
        Vector3 mass = Vector3.zero;
        foreach(Boid b in boids) {
            if(boid.Equals(b)) {

            } else {
                mass += b.transform.position;
            }
        }
        Vector3 pc =  mass/(boids.Count-1);
        return ((pc-boid.transform.position)/FACTOR).normalized;//set to 100
    }


    Vector3 BoundPosition(Boid boid) {
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
}
