using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartionedBoidController : MonoBehaviour {
    public BoidSettings settings;
    public Grid grid;
    List<PBoid> boids;

    public Transform target;//where to head toward?

    public int spawnCount;
    [HideInInspector] public int numBoids;
    public int maxFlockSize = 100;

    public bool DEBUG = false;
    public float spawnRadius = 10;
    [SerializeField] 
    PBoid prefab;
    
    public void Start() {
        boids = new List<PBoid>();
        grid.Initialize(1, new Vector3(-250, 250, 0),500);
        Spawn();
    }
    
    public void Spawn() {
        for(int i = 0 ; i < spawnCount ; i++) {
            Vector3 pos = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
            pos.z=0f;
            PBoid boid = Instantiate(prefab);
            boid.transform.position = pos;
            boid.transform.up = Random.insideUnitCircle;
            AddBoid(boid);
        }
    }

    void Update()
    {
        MoveBoids();
    }

    public void FindAllBoids() {
        if(boids == null) {
            boids = new List<PBoid>();
        }
        
        boids.Clear();
        boids.AddRange(FindObjectsOfType<PBoid>()); //cant do this because not monobehaviour
        numBoids = boids.Count;

        foreach(PBoid pb in boids) {
            pb.Initialize(this,null, target);
            grid.AddBoidToGrid(pb);
        }
    }

    public void AddBoid(PBoid boid) {
        if(boids == null) {
            boids = new List<PBoid>();
        }
        
        boid.Initialize(this,settings,target);
        numBoids++;
        boids.Add(boid);
        grid.AddBoidToGrid(boid);

    }

    public void MoveBoids() {
        CalculateCells();

        foreach(PBoid b in boids) {
            b.UpdateBoid();
            grid.Move(b);
        }
    }
    public void CalculateCells() {
        Cell c = grid.filledCells;
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
    public void CalculateAvoidence(Cell c)
    {
        PBoid b1 = c.head;
        for (int i = 0; i < c.numOfBoids; i++)
        {
            Vector3 separationHeading = new Vector3(0, 0, 0);

            int acNum = c.avoidCells.Count;
            for(int x = 0; x < acNum; x++)
            {
                Cell c2 = c.avoidCells[x];
                int c2Num = c2.numOfBoids;
                PBoid b2 = c2.head;
                for (int j = 0; j < c2Num; j++)
                {
                    if(b1 != b2)
                    {
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

    public void DeleteBoids()
    {
        foreach(PBoid b in boids)
        {
            Destroy(b.gameObject);
        }
        boids.Clear();
        numBoids = 0;
    }
}
