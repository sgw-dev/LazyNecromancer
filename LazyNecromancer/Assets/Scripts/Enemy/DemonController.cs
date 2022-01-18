using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    public GameObject player;
    public float maxRange;
    public float minRange;

    public float stepSize;
    public float moveSpeed;

    public float attackSpeed;

    private List<DirectionValue> moveDirections;

    private List<GameObject> demons;
    public LayerMask mask;

    // DanValues:
    private Spawner parentSpawner;
    int Health;



    public GameObject Room { get; set; }

    private DemonAnimationController DAC;
    private DirectionValue previous;

    public float diff;

    public float timeBetweenAttacks;
    public float attackChargeTime;

    public WaveSpell magicWave;
    //public GameObject magicWave;

    private bool attacking;
    private float attackTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //Temp health value
        this.Health = 50;

        player = GameObject.FindGameObjectWithTag("Player");
        moveDirections = new List<DirectionValue>();
        DAC = this.GetComponent<DemonAnimationController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        // If the attack has cooled down
        if(attackTimer > timeBetweenAttacks)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            // If the player is in range
            if(distanceToPlayer < maxRange)
            {
                WaveSpell wave = Instantiate(magicWave, transform.position, Quaternion.identity);
                //GameObject wave = Instantiate(magicWave, transform.position, Quaternion.identity);
                //wave.GetComponent<WaveSpell>().Play();
                wave.Play();
                attackTimer = 0f;
            }
        }
        else
        {
            attackTimer += Time.fixedDeltaTime;
        }
        demons = new List<GameObject>(GameObject.FindGameObjectsWithTag("Demon"));

        moveDirections.Clear();
        // Make a list of 16 possible move directions
        Vector3 forward = player.transform.position - gameObject.transform.position;
        forward = forward.normalized;
        moveDirections.Add(new DirectionValue(forward, 0));
        for (float i = 22.5f; i < 360; i += 22.5f)
        {
            moveDirections.Add(new DirectionValue(Quaternion.AngleAxis(i, Vector3.back) * forward, 0));
        }
        // Add the option to not move at all
        moveDirections.Add(new DirectionValue(Vector3.zero, 0));

        DirectionValue smallest = null;
        // Evaluate those directions
        foreach (DirectionValue dv in moveDirections)
        {
            // Each vector is normalized, so multiply it by stepSize
            Vector3 newPos = dv.Dir * stepSize;
            // Add the new move vector to the current position to get the new position
            newPos = newPos + gameObject.transform.position;

            // minimise this
            float distanceToPlayer = Vector3.Distance(newPos, player.transform.position);
            // If the player is too close, this is bad
            if(distanceToPlayer < minRange)
            {
                distanceToPlayer = (1/distanceToPlayer) * 1;
            }
            // maximise this
            float distanceToDemons = 0;
            foreach(GameObject demon in demons)
            {
                distanceToDemons += Vector3.Distance(newPos, demon.transform.position);
            }
            // Maxamise this
            float distanceToWall = 2.5f;
            //Debug.DrawLine(transform.position, transform.position+(dv.Dir * stepSize), Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dv.Dir, stepSize*2, mask);
            if(hit.collider != null)
            {
                distanceToWall = Vector3.Distance(transform.position, hit.transform.position);
                //Debug.DrawLine(transform.position, transform.position + (dv.Dir * stepSize * 2), Color.red);
            }
            else
            {
                //Debug.DrawLine(transform.position, transform.position + (dv.Dir * stepSize*2), Color.green);
            }
            dv.Value = distanceToPlayer + (3.5f / distanceToDemons) + (10f/distanceToWall);

            Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (dv.Dir * dv.Value));

            // Save the smallest value
            if (smallest == null || dv.Value < smallest.Value)
            {
                hit = Physics2D.CircleCast(transform.position, 0.5f, dv.Dir, stepSize * 2, mask);
                if (hit.collider == null)
                {
                    smallest = dv;
                }
                
            }
        }

        // Pick Smallest Direction
        // if the previous value has been set
        if(previous != null)
        {
            // If the difference is larger, use the new movement and update the previous
            if(Mathf.Abs(previous.Value - smallest.Value) > diff)
            {
                previous = new DirectionValue(smallest.Dir, smallest.Value);
            }
            else
            {
                //The new position is not different enough, so stay *still*
                previous.Dir = Vector3.zero;
            }
        }
        else
        {
            // This is the first time through, so just use the calculated values and set the previous
            previous = new DirectionValue(smallest.Dir, smallest.Value);
        }

        gameObject.GetComponent<Rigidbody2D>().velocity = previous.Dir * moveSpeed;
        Vector2 directionalInput = new Vector2(previous.Dir.x, previous.Dir.y);
        DAC.ResetState();
        DAC.InputDirection = directionalInput;
        DAC.UpdateAnimation();
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

    public void SetParentSpawner(Spawner spawner)
    {
        //this.parentSpawner = GameObject.FindGameObjectWithTag("ScriptRunner: Room 0");
        //this.parentSpawnerScript = parentSpawner.GetComponent<Spawner>();
        this.parentSpawner = spawner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CombatController cc = collision.GetComponent<CombatController>();
            this.Health -= 25;
            if (this.Health == 0)
            {
                parentSpawner.RemoveEnemyFromAlive(this.gameObject);
                parentSpawner.CheckIfEnemiesAlive();
                Destroy(gameObject);
            }
        }
        
    }
}
