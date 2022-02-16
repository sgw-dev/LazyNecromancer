using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSlimeController : SlimeProjectile {
    
    [SerializeField]
    int MaxHealth;
    int health;

    public int Damage;

    public float maxTravelDist;

    public Transform target;

    bool canMoveAgain;
    
    // void Update() {
    //     if(Input.GetKeyDown(KeyCode.Backslash)) {
    //         TakeDamage(5);
    //     }
    //     if(Input.GetKeyDown(KeyCode.D)) {
    //         GetComponent<Rigidbody2D>().AddForce(Vector2.right*10f);
    //     }
    // }

    //hides
    new void Awake() {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    //overrides start from base
    new void Start() {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player Sprite").transform;
        canMoveAgain=true;
        health= MaxHealth;
    }

    //overrides OnEnable from base
    new void OnEnable() {

    }

    
    public void Move() {
        if(canMoveAgain) {
            timer=0f;
            canMoveAgain=false;
            //get distance
            //make sure can only move x amount
            Vector3 direction = (target.position - transform.position).normalized;
            float dist = Vector3.Distance(transform.position,target.position);
            float percent = (maxTravelDist/dist);
            //Vector3 vel = ((Vector3)rb.velocity);
            if( dist < maxTravelDist) {
                Lob(transform.position,target.position);
            } else {
                Lob(transform.position,Vector3.Lerp(transform.position,target.position,percent));
            }
        }

    }

    
    public override void Impact() {
        anim.SetTrigger("Impact");
        canMoveAgain=true;

    }

    //overrides
    new private void OnTriggerEnter2D(Collider2D col) {
        if(!dmgDealt && col.CompareTag("Player")) {
            Debug.Log("Player was hit by a small slime!");
        }
    }

    public void DeathEnd() {
        Destroy(this.gameObject);
    }

    public void TakeDamage(int dmg) {
        health-=dmg;
        if(health <=0) {
            anim.SetTrigger("Die");
            canMoveAgain=false;
        }
    }
}
