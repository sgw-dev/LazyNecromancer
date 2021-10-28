using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoid : MonoBehaviour
{
    public Vector3 velocity;
    public float speed;

    public EnemyBoidController enemycon;

    SpriteRenderer spr;
    Animator anim;

    public bool Alive;

    void Awake() {
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Alive = true;
    }

    void Update()
    {
        if(!Alive) {
            Die();
            return;
        }
        
        transform.Translate(speed * velocity * Time.deltaTime,Space.World);
        
        if(velocity.x > 0) {
            spr.flipX = true;
        } else if(velocity.x < 0){
            spr.flipX = false;
        }

    }

    public void Die() {
        anim.SetTrigger("Death");
    }

    public void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.tag);
        if(other.CompareTag("Player")) {
            enemycon.Damage<MeshRenderer>(other.gameObject);
        }
    }
}
