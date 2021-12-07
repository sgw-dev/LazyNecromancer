using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] GameObject arms;
    //[SerializeField] GameObject weapon;

    [Space(10)]

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject manaProjectile;

    [Space(20)]

    [SerializeField] float attackCooldown = 0.1f;
    [SerializeField] float manaCooldown = 0.1f;

    bool isAttacking = false;
    bool isFiring = false;

    Vector3 firingPointAngle;

    SwordAnimation swordAnimation;
    PlayerController playerController;
    AnimationController animationController;

    void Start()
    {
        swordAnimation = GetComponentInChildren<SwordAnimation>();
        playerController = GetComponent<PlayerController>();
        animationController = GetComponentInChildren<AnimationController>();

        //weapon.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        else if (Input.GetButtonDown("Fire2") && !isFiring)
        {
            StartCoroutine(Shoot());
        }

        /*if (animationController.InputDirection == Vector2.up)
        {
            swordAnimation.transform.localPosition = new Vector3(0, 0.15f, 0);
            //swordAnimation.arcStartPos = 225;

            firingPointAngle = new Vector3(180, 0, 0);
        }
        else if (animationController.InputDirection == Vector2.left)
        {
            swordAnimation.transform.localPosition = new Vector3(-.1f, 0, 0);
            //swordAnimation.arcStartPos = 315;

            firingPointAngle = new Vector3(270, 0, 0);
        }
        else if (animationController.InputDirection == Vector2.right)
        {
            swordAnimation.transform.localPosition = new Vector3(.1f, 0, 0);
            //swordAnimation.arcStartPos = 135;

            firingPointAngle = new Vector3(90, 0, 0);
        }
        else if (animationController.InputDirection == Vector2.down)
        {
            swordAnimation.transform.localPosition = new Vector3(0, -.3f, 0);
            //swordAnimation.arcStartPos = 45;

            firingPointAngle = new Vector3(0, 0, 0);
        }*/

        //firePoint.transform.rotation = Quaternion.Euler(firingPointAngle);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DealDamage(collision.gameObject, 10);
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        
        //Vector3 weaponTransform = weapon.transform.localPosition;
        arms.SetActive(false);
        //weapon.SetActive(true);

        swordAnimation.PlayAnimation();
        yield return new WaitForSeconds(swordAnimation.Settings.Duration);

        //weapon.SetActive(false);
        arms.SetActive(true);
       // weapon.transform.localPosition = weaponTransform;

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private IEnumerator Shoot()
    {
        isFiring = true;

        Instantiate(manaProjectile, firePoint.position, firePoint.rotation);

        yield return new WaitForSeconds(manaCooldown);
        isFiring = false;
    }

    public void DealDamage(GameObject gameObject, int damageDealt)
    {
        /* Get the health of the gameobject
         * set it to health - damageDealt
         */
    }
}
