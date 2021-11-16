using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] StatResource health;
    
    Rigidbody2D playerRB;
    private Vector2 movement;

    public float moveSpeed = 20.0f;
    public float slowStop = 1.38f;

    private bool isDodging = false;
    private bool canDodge = true;

    public float dodgeDuration = .25f;
    public float dodgeSpeed = 45f;
    public float dodgeCooldown = 1.5f;

    public float dodgeStunTime = 0.25f;
    public float dodgeStunFactor = 0.25f;
    
    float speed;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();

        speed = moveSpeed;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        if (!isDodging)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            movement = new Vector2(inputX, inputY).normalized;

            if (Input.GetButtonDown("Jump") && canDodge)
            {
                StartCoroutine(Dodge());
            }
        }
    }

    private void HandleMovement()
    {
        if (movement.sqrMagnitude >= .00001f)
        {
            playerRB.velocity = movement * speed;
        }
        else if (movement.sqrMagnitude < .00001f)
        {
            playerRB.velocity /= slowStop;          // Controls feel slippery, this slows you down quicker
        }           
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        canDodge = false;

        speed = dodgeSpeed;

        yield return new WaitForSeconds(dodgeDuration);
        isDodging = false;
        speed = moveSpeed * dodgeStunFactor;

        yield return new WaitForSeconds(dodgeStunTime);
        speed = moveSpeed;

        yield return new WaitForSeconds(dodgeCooldown - dodgeStunTime);
        canDodge = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            playerRB.velocity = Vector2.zero;
        }
    }
}
