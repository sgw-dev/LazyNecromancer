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

    void OnTriggerEnter2D (Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
            // put code here for projectile death anim
        }
        else if (collider2D.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            // put code here for projectile death anim
            // and DealDamage() from combat controller
        }
    }
}
