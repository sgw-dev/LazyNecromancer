using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour {

    public AnimationCurve travelarc;
    [SerializeField]
    float TimeUntilImpact;
    float timer;

    public float maxHeight; 
    
    Animator anim;

    public delegate void ReturnToPool(SlimeProjectile glob);
    public ReturnToPool putback;
    
    GameObject inRange;

    bool dmgDealt;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    void Start() {
        Reset();
    }

    void OnEnable() {
        Reset();
    }

    void Update() {
        // if(Input.GetKeyDown(KeyCode.Alpha2)) {
        //     Reset();
        //     Lob(transform.position, GameObject.Find("Player Sprite").transform.position);
        // }
        // if(Input.GetKeyDown(KeyCode.Alpha3)) {
        //     Reset();
        //     Hurl(transform.position, GameObject.Find("Player Sprite").transform.position);
        // }
    }

    public void Done() {
        putback(this);
    }

    public void Reset() {
        dmgDealt = false;
        timer = 0f;
        anim.SetBool("Flying",true);
    }

    public void ThrowProjectile(Vector3 start,Vector3 end) {
        
        if(Random.value > .5) {
            //Task.Run(async () => await Lob(start,end));
            Lob(start,end);
        } else {
            //Task.Run(async () => await Hurl(start,end));
            Hurl(start,end);
        }
    }

    bool _animating;
    async Task Lob(Vector3 start,Vector3 end) {
        if(!_animating) {
            _animating=true;
            while(timer < TimeUntilImpact) {
                float percent = timer/TimeUntilImpact;
                float heightpercent = travelarc.Evaluate(percent);

                transform.position = Vector3.Lerp(start,end,percent) +new Vector3(0,+heightpercent * maxHeight, 0f);

                timer+= Time.deltaTime;
                await Task.Yield();
            }
            Impact();
            _animating=false;
        }
    }

    async Task Hurl(Vector3 start,Vector3 end) {
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

    public void Impact() {
        anim.SetBool("Flying",false);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(!dmgDealt && col.CompareTag("Player")) {
            dmgDealt=true;
            Debug.Log("Player was hit by a glob!");
        }
    }


}