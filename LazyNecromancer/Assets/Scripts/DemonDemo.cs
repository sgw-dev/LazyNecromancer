using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDemo : MonoBehaviour
{

    private Spawner parentSpawner;
    //private Spawner parentSpawnerScript;

    private int spawnerIndex;

    int Health;
    // Start is called before the first frame update
    void Start()
    {
        this.Health = 50;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetParentSpawner(Spawner spawner)
    {
        //this.parentSpawner = GameObject.FindGameObjectWithTag("ScriptRunner: Room 0");
        //this.parentSpawnerScript = parentSpawner.GetComponent<Spawner>();
        this.parentSpawner = spawner;
    }

    public void SetSpawnerIndex(int index)
    {
        this.spawnerIndex = index;
    }

    public int GetSpawnerIndex()
    {
        return this.spawnerIndex;
    }

    private void OnMouseDown()
    {
        print("You clicked");
        this.Health -= 25;
        if (this.Health == 0) {
            parentSpawner.RemoveEnemyFromAlive(this.gameObject);
            parentSpawner.CheckIfEnemiesAlive();
            Destroy(gameObject);
        }
        //Destroy(this);
        //parentSpawnerScript.CheckIfEnemiesAlive();
    }
}
