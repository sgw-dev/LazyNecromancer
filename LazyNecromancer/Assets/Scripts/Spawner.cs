using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private List<int> enemyList;
    private ArrayList aliveEnemies;
    public int totalEnemies;
    private Transform[] spawnCircles;

    private int enemyCounter;

    public GameObject Demon;
    public GameObject Rat;

    public GameObject[] SpawnCircleGroups;
    private GameObject SpawnCircleGroup;

    public int RoomDepth { get; set; }

    public GameObject[] ClutterGroups;
    public Transform[] Corners;

    public Color disabledCircleColor;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void StartSpawning()
    {
        int rand = Random.Range(0, SpawnCircleGroups.Length);
        SpawnCircleGroup = Instantiate(SpawnCircleGroups[rand], this.transform);

        this.findSpawnCircles();
        this.determineTotalEnemies();
        this.aliveEnemies = new ArrayList();

        //Spawn Clutter
        foreach(Transform corner in Corners)
        {
            rand = Random.Range(0, 2);
            if (rand == 1)
            {
                rand = Random.Range(0, ClutterGroups.Length);
                Instantiate(ClutterGroups[rand], corner);
            }
        }
        
        /*
        this.fillEnemyList();
        this.aliveEnemies = new ArrayList();
        this.determineTotalEnemies();


        this.totalEnemies = 6;
        this.enemyCounter = 0;


        //Return spawnCircle list, instead of auto-setting?
        this.findSpawnCircles();

        //Need to call this only when the player actually enters the room
        this.PlayerEntersRoom();*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Fills the enemy list with enemies based on room depth
    private void fillEnemyList()
    {
        this.enemyList = new List<int>() { 2, 2, 2, 2, 2, 2 };
    }

    //Determine total enemies based on the room depth
    private void determineTotalEnemies()
    {
        Debug.Log("Room Depth of " + gameObject.name + " is " + RoomDepth);
        if(RoomDepth > 2)
        {
            this.totalEnemies = spawnCircles.Length * 2;
        }
        else
        {
            this.totalEnemies = spawnCircles.Length;
        }
    }

    //Need to select only the spawn circles
    private void findSpawnCircles()
    {
        //Make sure that the spawn circles get a tag to denote them specifically.
        //this.spawnCircles = GameObject.FindGameObjectsWithTag("SpawnCircle: Room 0");
        //this.spawnCircles = transform.GetComponentsInDirectChildren<Transform>(SpawnCircleGroup.transform);
        spawnCircles = new Transform[SpawnCircleGroup.transform.childCount];
        for(int i = 0; i<SpawnCircleGroup.transform.childCount; i++)
        {
            spawnCircles[i] = SpawnCircleGroup.transform.GetChild(i);
        }
    }

    public void PlayerEntersRoom()
    {
        this.spawnEnemyGroup();
    }

    private void spawnEnemyGroup()
    {
        if (this.totalEnemies > 0)
        {
            foreach (Transform spawnCircle in this.spawnCircles)
            {
                GameObject spawnedDemon = Instantiate(Demon, spawnCircle);
                spawnedDemon.transform.localPosition = Vector3.zero;
                var demonScript = spawnedDemon.GetComponent<DemonController>();
                demonScript.SetParentSpawner(this);
                this.aliveEnemies.Add(spawnedDemon);
                /*
                var random = Random.Range(0, this.enemyList.Count);
                var randomPull = this.enemyList[random];

                if (randomPull == 1)
                {
                    var spawnedRat = Instantiate(Rat, spawnCircle.position, Quaternion.identity);
                    spawnedRat.tag = "Room 0";
                    this.aliveEnemies.Add(spawnedRat);
                }
                if (randomPull == 2)
                {
                    var spawnedDemon = Instantiate(Demon, spawnCircle.position, Quaternion.identity);
                    spawnedDemon.tag = "Room 0";
                    this.aliveEnemies.Add(spawnedDemon);


                    var demonScript = spawnedDemon.GetComponent<DemonController>();

                    //Need to make the other version of the actual script for functionality.
                    //Will be part of handling enemy death.
                    //demonScript.SetParentSpawner(this);
                    demonScript.SetParentSpawner(this);
                    // **Spencer ** Enemy counter is never incremented
                    //demonScript.SetSpawnerIndex(this.enemyCounter);
                }
                */
                //Assuming something is always spawned.
                this.totalEnemies -= 1;
            }
        }
        else
        {
            print("You've killed all the enemies!");
        }
    }

    public void CheckIfEnemiesAlive()
    {
        
        print("Checking if enemies dead");
        print("Alive enemies count: " + this.aliveEnemies.Count);
        if (this.aliveEnemies.Count == 0)
        {
            //print("Enemies found to be dead.");
            //this.spawnEnemyGroup();
            if(totalEnemies > 0)
            {
                this.spawnEnemyGroup();
            }
            else
            {
                this.GetComponent<DoorManager>().RoomCleared();
                foreach(Transform circle in spawnCircles)
                {
                    circle.GetComponent<SpriteRenderer>().color = disabledCircleColor;
                }
            }
            
        }
    }

    //Pretty sure this method is a type-casting time-bomb. Better way to do this?
    //Should set an Array of enemies, and have the enemy remember it's "AliveEnemiesIndex".
    //It should then call a script in here, that removes the enemy from the arrayindex specified.
    //When I frist tried to do this, I accidentally called PlayerEntersRoom
    //before creating the Array of enemies
    public void RemoveEnemyFromAlive(GameObject enemy)
    {
        /*
        foreach(DemonDemo enemy in this.aliveEnemies)
        {
            if(enemy.GetSpawnerIndex() == index)
            {
                this.aliveEnemies.Remove(enemy);
            }
        }
        */
        // **Spencer** Find the gameobject in the array and remove it
        this.aliveEnemies.Remove(enemy);
    }
}
