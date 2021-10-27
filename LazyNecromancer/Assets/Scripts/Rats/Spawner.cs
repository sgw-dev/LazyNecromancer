using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //spawning
    public float spawnRadius = 10;
    public int spawnCount = 10;
    [SerializeField] int fireRate = 2;
    int counter;
    [HideInInspector] public bool spawnBoids = true;

    //Boid Initialize 
    [SerializeField] Sprite[] sprites;
    private Sprite currentSprite;
    [SerializeField] List<Color> colors;
    public Color currentColor = Color.white;

    //Grid
    [SerializeField] Vector3 gridOrigin = new Vector3(-250, 250, 0);
    public  int size = 500;
    public int cellSize = 1;

    //cached
    //[SerializeField] UIController uIController;
    [SerializeField] PBoid prefab;
    //[SerializeField] CreateBorder border;
    PartionedBoidController boidManager;



    void Awake()
    {
        /*
        if (sprites.Length != 0)
            currentSprite = sprites[0];
        else
            currentSprite = null;

        if (colors.Count != 0)
            currentColor = colors[0];
        else
            currentColor = Color.white;
        */
        boidManager = GetComponent<PartionedBoidController>();
    }

    public void Start()
    {
        boidManager.grid.Initialize(cellSize, gridOrigin, size);
        //border.Initialize(gridOrigin, size);
        SpawnAll();
    }

    public void SpawnAll()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
            PBoid boid = Instantiate(prefab);
            boid.transform.position = pos;
            boid.transform.up = Random.insideUnitCircle;
            boidManager.AddBoid(boid);
        }

        //uIController.UpdateNumOfBoids();
    }
    public void Update()
    {
        /*if (spawnBoids)
            SpawnOne();
        if (counter > 0)
            counter--;*/
    }
/*
    public void SpawnOne()
    {
        if (Input.GetMouseButton(0) && counter <= 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            camPos.z = 0;

            Boid2 boid = Instantiate(prefab);
            boid.transform.position = camPos;
            boid.transform.up = Random.insideUnitCircle;

            boidManager.AddBoid(boid);
            uIController.UpdateNumOfBoids();
            counter = fireRate;
        }
    }
*/
    public void ResetBoids()
    {
        boidManager.DeleteBoids();
        //uIController.UpdateNumOfBoids();
    }
}
