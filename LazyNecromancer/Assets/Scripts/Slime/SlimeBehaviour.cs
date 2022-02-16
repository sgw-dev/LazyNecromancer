using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SlimeBehaviour : MonoBehaviour {

    [SerializeField]
    int MaxHealth;
    int Health;

    // [SerializeField]
    // [Range(1,10)]
    // float bounds=9f;
    public Vector3 spawnpoint ;

    [SerializeField]
    float trailSpawnTime;
    float trailspawntimer;
    public Transform trailspawnpoint;
    private Queue<SlimeTrailDecay> trail_pool;

    [SerializeField]
    [Tooltip("milliseconds")]
    int smallSlimeSpawnPause;
    [SerializeField]
    int numberOfSmallSlimes;
    [SerializeField]
    GameObject smallSlimePrefab;
    [SerializeField]
    [Range(0f,20f)]
    float spawnRadius;

    Queue<SmallSlimeController> smallSlimePool;


    public int maxGlobs;
    public GameObject globPrefab;
    private Queue<SlimeProjectile> glob_pool;

    public GameObject trailprefab;
    public int trailPoolCount;
    public float knockbackForce;
    
    [SerializeField]
    float RangedAttackTimer;
    float rangedTimer;

    [Range(1f,20f)]
    public float moveSpeed;

    [Range(.01f,1f)]
    [SerializeField]
    [Tooltip("Percent of the original behaviourChangeTime")]
    float BehaviourChangePercent;
    [SerializeField]
    float behaviourChangeTime;
    float behaviourtimer;//timer to do something else?

    [SerializeField]
    float SlideMoveTime; //exposed time for timer
    float moveTimer;     //class tracked timer
    bool _move;          //semaphore for slime movement function

    [Space(50)]
    public Material lit;

    //flags for behaviour
    public bool playerSighted;
    public bool playerInAttackRange;
    public bool nearDeath;
    public Vector3 positionOfInterest;

    //sensory flags
    public SlimeAttackRange chargeRangeScript;
    public SlimeAttackRange rangedAttackScript;
    public SlimeAttackRange damageScript;

    [SerializeField]
    float MeleeAttacksPerSecond;
    float attackTimer;

    Animator anim;
    bool dead;

    void Start() {

        anim = GetComponent<Animator>();
        dead = false;
        Health = MaxHealth;

        if(chargeRangeScript == null) {
            Debug.LogError("chargeRangeScript is not set");
        }
        if(rangedAttackScript == null) {
            Debug.LogError("rangedAttackScript is not set");
        }
        
        trail_pool = new Queue<SlimeTrailDecay>();
        for(int i = 0 ; i < trailPoolCount; i++ ) {
            var tmp = GameObject.Instantiate(trailprefab,null);
            var trailscript = tmp.GetComponent<SlimeTrailDecay>();
            trailscript.returntopool = ReturnToPool;
            tmp.SetActive(false);
            trail_pool.Enqueue(trailscript);

        }
        
        glob_pool = new Queue<SlimeProjectile>();
        for(int i = 0 ; i < maxGlobs ; i++) {
            var tmp = GameObject.Instantiate(globPrefab,null);
            SlimeProjectile projectilescript = tmp.GetComponent<SlimeProjectile>();
            projectilescript.putback = ReturnToPool;
            tmp.SetActive(false);
            glob_pool.Enqueue(projectilescript);
        }

        smallSlimePool = new Queue<SmallSlimeController>();
        for(int i = 0 ; i < numberOfSmallSlimes ; i++) {
            var tmp = GameObject.Instantiate(smallSlimePrefab,null);
            SmallSlimeController ssscript = tmp .GetComponent<SmallSlimeController>();
            tmp.SetActive(false);
            smallSlimePool.Enqueue(ssscript);
        }

        positionOfInterest = transform.position;
        _move = false;

        playerSighted = playerInAttackRange = nearDeath = false; 
        //set delegates
        chargeRangeScript.setflag   = SetPlayerInAttackRange;
        chargeRangeScript.clearflag = UnSetPlayerInAttackRange;
        chargeRangeScript.PointOfIntereset = SetTarget;

        rangedAttackScript.setflag   = SetPlayerSighted;
        rangedAttackScript.clearflag = UnSetPlayerSighted;
        rangedAttackScript.PointOfIntereset = SetTarget;

        //cheating here, this doesnt make a lot of sense but works
        damageScript.setflag = Attack;
        damageScript.clearflag = Nothing;
        damageScript.PointOfIntereset = SetTarget;
    }

    void Update() {
        if(dead) {
            return;
        }
        
// #if UNITY_EDITOR
//         if(Input.GetKeyDown(KeyCode.Minus)) {
//             TakeDamage(21);
//         }
// #endif

        ChangeBehaviour();
        PlaceTrail();
        behaviourtimer  += Time.deltaTime;
        attackTimer     += Time.deltaTime;
        trailspawntimer += Time.deltaTime;
        rangedTimer     += Time.deltaTime;
    }

    void SpawnAnimation() {
        Debug.Log("Slime is here");
    }
    
    //called by trigger in child object
    //see damageScript delegate 
    void Attack() {
        if(attackTimer > 1f/MeleeAttacksPerSecond) {
            attackTimer = 0f;
            Debug.Log("Attacking");
        }
    }

    //Called when hp <=0
    void Die() {
        dead=true;
        //other cleanup stuff
        GetComponent<Collider2D>().enabled=false;
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        LetSlimesLoose();
        anim.SetTrigger("Die");
        Debug.Log("Big Slime is dead.");
    }

    //called once big slime dies,
    //randomly lobs small slimes in spawnRadius
    //waits 5 seconds then deacrivates this gameObject
    async void LetSlimesLoose() {
        while(smallSlimePool.Count>0) {
            var slime = smallSlimePool.Dequeue();
            slime.transform.position = transform.position;
            slime.gameObject.SetActive(true);
            
            slime.ThrowProjectile(transform.position,transform.position + spawnRadius * (new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0f)));
            await Task.Delay(smallSlimeSpawnPause);
        }

        await Task.Delay(5000);//5 second delay
        this.enabled=false;
        gameObject.SetActive(false);
    }

    //returns projectiles to pool
    public void ReturnToPool(SlimeProjectile glob) {
        glob.gameObject.SetActive(false);
        glob_pool.Enqueue(glob);
    }

    //returns trail to queue
    public void ReturnToPool(SlimeTrailDecay trail) {
        trail.gameObject.SetActive(false);
        trail_pool.Enqueue(trail);
    }

    //spawns trail object on the gameObject position
    void PlaceTrail() {
        if(trailspawntimer>trailSpawnTime){
            trailspawntimer=0f;
            var trailscript = trail_pool.Dequeue();
            trailscript.transform.position = trailspawnpoint.position;
            trailscript.gameObject.SetActive(true);
        }
    }

    //move the slime toward a position without reguards to physics
    //see small slime's script if physics should be required
    async void MoveToward(Vector3 t) {

        if(!_move) {
            
            _move = true;  //lock
            Vector3 start = transform.position;
            Vector3 end   = t;
            moveTimer = 0f;

            while(moveTimer < SlideMoveTime) {
                
                moveTimer += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end,moveTimer/SlideMoveTime);
                await Task.Yield();
            }
            transform.position = end;
            _move = false; //unlock
        }
    }

    //actually need this for now
    void Nothing() {}

    //Timer function to call the chain of "if else" statements
    //for change in slimes current state
    void ChangeBehaviour() {
        if(behaviourtimer > behaviourChangeTime)  {
            behaviourtimer = 0f;
            //do something here
            DoSomething();
        }
    }

    //Throw object from ranged attack pool at player,
    //or positionOfInterest
    void RangedAttack() {
        if(rangedTimer > RangedAttackTimer) {
            rangedTimer = 0f;
            var globule = glob_pool.Dequeue();
            globule.gameObject.SetActive(true);
            globule.ThrowProjectile(transform.position,positionOfInterest);
        }
    }

    void DoSomething() {
        if(playerSighted && playerInAttackRange && nearDeath) {
            //tackle the player
            MoveToward(positionOfInterest);
        } else if(playerSighted && playerInAttackRange && !nearDeath) {
            //tackle tha player
            MoveToward(positionOfInterest);
        } else if(playerSighted && !playerInAttackRange && nearDeath) {
            //Throw globs, faster
            RangedAttack();
        } else if(playerSighted && !playerInAttackRange && !nearDeath) {
            //Throw globs
            RangedAttack();
        } else if(!playerSighted && playerInAttackRange && nearDeath) {
            //look for player, this state doesnt make sense unless player can be hidden from range
            MoveToward(positionOfInterest);
        } else if(!playerSighted && playerInAttackRange && !nearDeath) {
            //look for player, this state doesnt make sense unless player can be hidden from range
            MoveToward(positionOfInterest);
        } else if(!playerSighted && !playerInAttackRange && nearDeath) {
            //float f = Random.value;
            //MoveToward(new Vector3(f,f,0)+transform.position);
            MoveToward(Wander());
        } else if(!playerSighted && !playerInAttackRange && !nearDeath) {
            //float f = Random.value;
            //MoveToward(new Vector3(f,f,0)+transform.position);
            MoveToward(Wander()); 
        } else {
            Debug.LogError("Impossible state");
        }
    }

    public Vector3 Wander() {
        float x = Random.Range(spawnpoint.x-10f,spawnpoint.x+10f);
        float y = Random.Range(spawnpoint.y-10f,spawnpoint.y+10f);
        return Vector3.Lerp(transform.position, new Vector3(x,y,0f),.1f);         
    }

    public void TakeDamage(int damage) {
        
        Health-= damage;
        
        if(Health < (int)(MaxHealth*.2f)) {
            nearDeath = true;
            behaviourChangeTime *= BehaviourChangePercent;
        }

        if(Health <=0 ) {
            Die();
        }
    }
    
    void OnCollisionEnter2D(Collision2D col) {
        if(col.transform.CompareTag("Player")) {
            Vector2 KnockBackDirection = col.GetContact(0).point - ((Vector2)transform.position);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(KnockBackDirection.normalized * knockbackForce ,ForceMode2D.Impulse);
            Debug.Log("Player got tackled by the slime!");
        }
    }

    //functions to delegate to the radius objects
    public void SetPlayerSighted()         { playerSighted = true; }
    public void UnSetPlayerSighted()       { playerSighted = false; }
    public void SetPlayerInAttackRange()   { playerInAttackRange = true; }
    public void UnSetPlayerInAttackRange() { playerInAttackRange = false; }

    public void SetTarget(Vector3 pos)     { positionOfInterest = pos;}

}