using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SlimeBehaviour : MonoBehaviour {

    [SerializeField]
    int MaxHealth;
    int Health;



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
    float SlideMoveTime;//exposed time for timer
    float moveTimer;//class tracked timer
    bool _move;//semaphore for slime movement function
    
    //Probably get rid of these eventually
    [Space(100)]
    public bool DEBUG;
    public Material unlit;
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

    bool dead;
    void Start() {
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



        if(DEBUG) {
            GetComponent<SpriteRenderer>().sharedMaterial = unlit;
        } else {
            GetComponent<SpriteRenderer>().sharedMaterial = lit;
        } 

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
        
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Minus)) {
            TakeDamage(21);
        }
#endif

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
    
    void Attack() {
        if(attackTimer > 1f/MeleeAttacksPerSecond) {
            attackTimer = 0f;
            Debug.Log("Attacking");
        }
    }

    void Die() {
        dead=true;
        //other cleanup stuff
        GetComponent<Collider2D>().enabled=false;
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        LetSlimesLoose();
        Debug.Log("Dead");
    }

    async void LetSlimesLoose() {
        while(smallSlimePool.Count>0) {
            var slime = smallSlimePool.Dequeue();
            slime.transform.position = transform.position;
            slime.gameObject.SetActive(true);
            
            slime.ThrowProjectile(transform.position,transform.position + spawnRadius * (new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0f)));
            await Task.Delay(smallSlimeSpawnPause);
        }
        this.enabled=false;
        gameObject.SetActive(false);
    }

    public void ReturnToPool(SlimeProjectile glob) {
        glob.gameObject.SetActive(false);
        glob_pool.Enqueue(glob);
    }

    public void ReturnToPool(SlimeTrailDecay trail) {
        trail.gameObject.SetActive(false);
        trail_pool.Enqueue(trail);
    }


    void PlaceTrail() {
        if(trailspawntimer>trailSpawnTime){
            trailspawntimer=0f;
            var trailscript = trail_pool.Dequeue();
            trailscript.transform.position = trailspawnpoint.position;
            trailscript.gameObject.SetActive(true);
        }
    }

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

    void ChangeBehaviour() {
        if(behaviourtimer > behaviourChangeTime)  {
            behaviourtimer = 0f;
            //do something here
            DoSomething();
        }
    }

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
            //attack, last ditch effor
            MoveToward(positionOfInterest);
            // current = Attack;
        } else if(playerSighted && playerInAttackRange && !nearDeath) {
            //attack normally
            MoveToward(positionOfInterest);
            // current = Attack;
        } else if(playerSighted && !playerInAttackRange && nearDeath) {
            //move toward player frantically
            RangedAttack();
        } else if(playerSighted && !playerInAttackRange && !nearDeath) {
            //move toward player
            //MoveToward(positionOfInterest);
            // current = Nothing;
            RangedAttack();
        } else if(!playerSighted && playerInAttackRange && nearDeath) {
            //look for player, this state doesnt make sense unless player can be hidden
            MoveToward(positionOfInterest);
            // current = Nothing;
        } else if(!playerSighted && playerInAttackRange && !nearDeath) {
            //look for player, this state doesnt make sense unless player can be hidden
            MoveToward(positionOfInterest);
            // current = Nothing;
        } else if(!playerSighted && !playerInAttackRange && nearDeath) {

            float f = Random.value;
            MoveToward(new Vector3(f,f,0));
        } else if(!playerSighted && !playerInAttackRange && !nearDeath) {

            float f = Random.value;
            MoveToward(new Vector3(f,f,0));
        } else {
            Debug.LogError("Impossible state");
        }
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