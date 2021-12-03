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


    //hide from base class
    new void Awake() {
        base.Awake();
        
    }

    //hide start from base class
    void Start() {
        
        target = GameObject.Find("Player Sprite").transform;
        canMoveAgain=true;
    }

    //hide reset from base class
    void OnEnable() {

    }


    bool canMoveAgain;
    public void Move() {
        if(canMoveAgain) {
            timer=0f;
            canMoveAgain=false;
            //get distance
            //make sure can only move x amount
            Vector3 direction = (target.position - transform.position).normalized;
            float dist = Vector3.Distance(transform.position,target.position);
            float percent = (maxTravelDist/dist);
            
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


    private void OnTriggerEnter2D(Collider2D col) {
        if(!dmgDealt && col.CompareTag("Player")) {
            Debug.Log("Player was hit by a small slime!");
        }
    }
}
