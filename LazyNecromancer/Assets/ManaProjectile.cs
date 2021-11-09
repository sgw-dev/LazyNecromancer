using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaProjectile : MonoBehaviour
{
    [SerializeField] float speed = 8f;

    Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = -transform.up * speed;
    }

    void OnTriggerEnter2D (Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
            // put code here for projectile death anim
        }
        else if (trigger.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            // put code here for projectile death anim
            // and DealDamage() from combat controller
        }
    }
}
