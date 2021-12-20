using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyGrid))]
public class EnemyBoidController : MonoBehaviour
{
    public GameObject enemyPrefab;
    [Range(0,100)]
    public int numberOfBoids;
    public int numBoids;
    public float boidSpeed;
    public int boidDamage;
    [Range(0,10)]
    public int MaxAttacksPerSecond;
    [Space(20)]
    [Range(-300f,300f)] public int Xmax;
    [Range(-300f,300f)] public int Xmin;
    [Range(-300f,300f)] public int Ymax;
    [Range(-300f,300f)] public int Ymin;
    [Range(1,50)]       public int cellNum;

    [HideInInspector]
    public List<EnemyBoid> boids;

    public Transform target;

    public  float behaviourTimerMax;
    private float behaviourTimer;
    public  float swapvalue;
    private float swappedvalue;


    float attacktimer;

    public EnemyGrid grid;

    public BoidSettings settings;

    public GameObject btarget;

    void Start() {
        attacktimer = 0f;
        behaviourTimer = 0f;
        grid.Initialize(cellNum,Xmin,Xmax,Ymin,Ymax);
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
            AddBoid(boidscript);
        }
    }

    public void AddBoid(EnemyBoid boid) {
        if(boids == null) {
            boids = new List<EnemyBoid>();
        }
        
        boid.Initialize(this,settings,target);
        numBoids++;
        boids.Add(boid);
        grid.AddBoidToGrid(boid);

    }


    public Vector3 RandomInBounds() {
        //any number less than max/min should work i.e. 75%
        return new Vector3(
            transform.position.x + Random.Range(Xmin*.75f,Xmax*.75f),
            transform.position.y + Random.Range(Ymin*.75f,Ymax*.75f),
            0
        );
    }

    void Update() {
        BehaviourChange();
        MoveBoids();
        attacktimer+=Time.deltaTime;
    }

    public void MoveBoids() {
        CalculateCells();

        foreach(EnemyBoid b in boids) {
            b.UpdateBoid();
            b.velocity += BoundPosition(b);
            grid.Move(b);
        }
    }

    public void CalculateCells() {
        EnemyCell c = grid.filledCells;
        int fcNum = grid.numOfFilled;

        for(int i = 0 ; i < fcNum; i++ ) {
            c.ResetTotals();
            c= c.nextCell;
        }
        
        c = grid.filledCells;

        for(int i = 0; i < fcNum; i++ ) {
            c.AddToCells();
            CalculateAvoidence(c);
            c = c.nextCell;
        }
    }

    public void CalculateAvoidence(EnemyCell c) {

        EnemyBoid b1 = c.head;
        for (int i = 0; i < c.numOfBoids; i++) {

            Vector3 separationHeading = new Vector3(0, 0, 0);

            int acNum = c.avoidCells.Count;
            for(int x = 0; x < acNum; x++) {

                EnemyCell c2 = c.avoidCells[x];
                int c2Num = c2.numOfBoids;
                EnemyBoid b2 = c2.head;
                for (int j = 0; j < c2Num; j++) {
                    if(b1 != b2) {
                        Vector3 offset = new Vector3(b2.position.x - b1.position.x, b2.position.y - b1.position.y, 0);
                        float sqrDst = offset.x * offset.x + offset.y * offset.y;
                        offset = new Vector3( offset.x / sqrDst, offset.y / sqrDst, 0);
                        separationHeading = new Vector3(separationHeading.x + offset.x, separationHeading.y + offset.y, 0);
                    }
                    b2 = b2.nextBoid;
                }
            }
            b1.avgAvoidanceHeading = -separationHeading;
            b1 = b1.nextBoid;
        }
    }
#if UNITY_EDITOR
    //testing only    
    bool tt=false;

    public void ToggleTargetButton() {
        
        if(tt) {
            ToggleTarget(btarget);
        } else {
            ToggleTarget(null);
        }
        tt=!tt;
    
    }
#endif

    public void ToggleTarget(GameObject t) {

        if(t==null) {
            foreach(EnemyBoid b in boids) {
                b.target = null;
            }
        } else {
            foreach(EnemyBoid b in boids) {
                b.target = t.transform;
            }
        }
    }

    //change the boid parameters via script
    void BehaviourChange() {

        behaviourTimer+=Time.deltaTime;

        if(behaviourTimer >= behaviourTimerMax) {
            behaviourTimer=0f;
            // swappedvalue = Separation;
            // Separation = swapvalue;
            // swapvalue = swappedvalue;
        }
    }

    //Really push the boid back away from edges of grid
    Vector3 BoundPosition(EnemyBoid boid) {

        Vector3 v = Vector3.zero;

        if(boid.transform.position.x < (Xmin+1)) {
            v.x = 10f;
        } else if(boid.transform.position.x > (Xmax-1)) {
            v.x = -10f;
        } else if(boid.transform.position.y < (Ymin+1)) {
            v.y = 10f;
        } else if( boid.transform.position.y > (Ymax-1)) {
            v.y = -10f;
        }
        
        return v;
    }

    //Function to deal damage to the player
    public void Damage<T>(GameObject obj) {

        if(attacktimer < (1.0f/MaxAttacksPerSecond)) {
            return;
        }
        
        attacktimer = 0f; //reset timer

        T healthscript = GetComponent<T>();

        if(healthscript != null) {
            //healthscript.takeDmg();
            //Debug.Log(obj.name + " took " + boidDamage + ".");
        }
    }

#if UNITY_EDITOR
    void OnValidate() {
        //Vector3 corner = new Vector3(Xmin,Ymin);
        GetComponent<EnemyGrid>().Initialize(cellNum,Xmin,Xmax,Ymin,Ymax);
    }
#endif

}
