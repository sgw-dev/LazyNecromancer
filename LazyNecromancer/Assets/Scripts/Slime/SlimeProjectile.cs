using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour {

    [SerializeField]
    protected AnimationCurve travelarc;
    [SerializeField]
    protected float TimeUntilImpact;
    protected float timer;

    [SerializeField]
    protected float maxHeight; 
    
    protected Animator anim;

    public delegate void ReturnToPool(SlimeProjectile glob);
    public ReturnToPool putback;
    

    protected GameObject inRange;

    protected bool dmgDealt;
    protected Rigidbody2D rb;
    private protected void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private protected void Start() {
        Reset();
    }

    private protected void OnEnable() {
        Reset();
    }

    private protected void Update() {
        // if(Input.GetKeyDown(KeyCode.Alpha2)) {
        //     Reset();
        //     Lob(transform.position, GameObject.Find("Player Sprite").transform.position);
        // }
        // if(Input.GetKeyDown(KeyCode.Alpha3)) {
        //     Reset();
        //     Hurl(transform.position, GameObject.Find("Player Sprite").transform.position);
        // }
    }

    private protected void Done() {
        putback(this);
    }

    private protected void Reset() {
        dmgDealt = false;
        timer = 0f;
        anim.SetBool("Flying",true);
    }

    public virtual void ThrowProjectile(Vector3 start,Vector3 end) {
        
        if(Random.value > .5) {
            //Task.Run(async () => await Lob(start,end));
            Lob(start,end);
        } else {
            //Task.Run(async () => await Hurl(start,end));
            Hurl(start,end);
        }
    }

    bool _animating;
    private protected async Task Lob(Vector3 start,Vector3 end) {
        if(!_animating) {
            _animating=true;
            while(timer < TimeUntilImpact) {
                float percent = timer/TimeUntilImpact;
                float heightpercent = travelarc.Evaluate(percent);
                Vector3 vel = (rb!=null) ? (new Vector3(rb.velocity.x,rb.velocity.y,0f)) : Vector3.zero;
                transform.position = Vector3.Lerp(start,end + vel,percent) +new Vector3(0,+heightpercent * maxHeight, 0f);

                timer+= Time.deltaTime;
                await Task.Yield();
            }
            Impact();
            _animating=false;
        }
    }

    private protected async Task Hurl(Vector3 start,Vector3 end) {
        if(!_animating) {
            _animating=true;
            while(timer < TimeUntilImpact) {
                float percent = timer/TimeUntilImpact;
                transform.position = Vector3.Lerp(start,end,percent) ;
                timer+= Time.deltaTime;
                await Task.Yield();
            }
            Impact();
            _animating=false;
        }
    }

    public virtual void Impact() {
        anim.SetBool("Flying",false);
    }

    private protected void OnTriggerEnter2D(Collider2D col) {
        if(!dmgDealt && col.CompareTag("Player")) {
            dmgDealt=true;
            Debug.Log("Player was hit by a glob!");
        }
    }


}