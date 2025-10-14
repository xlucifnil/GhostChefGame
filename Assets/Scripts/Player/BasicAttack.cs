using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] GameObject AttackHitBoxHorizontal; // Default faces right
    [SerializeField] GameObject AttackHitBoxVertical;
    [SerializeField] GameObject StretchyAttackHitBoxHorizontal; // Default faces right
    [SerializeField] GameObject StretchyAttackHitBoxVertical;
    public float yOffset = 0;
    public float xOffset = 0;
    public float strechyYOffset = 0;
    public float strechyXOffset = 0;
    public float timeBetweenAttacks = 1;
    public float energizingAttackModifier = 0.1f;
    float currentTimeBetween = 0;
    GameObject player;
    public GameObject testEffect;
    bool attackLocked;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        currentTimeBetween -= Time.deltaTime;
        if (Time.timeScale != 0f && !attackLocked)
        {
            if (currentTimeBetween <= 0)
            {

                gameObject.GetComponent<PlayerMovement>().animator.SetBool("Attacking", false);
                if (Input.GetButtonDown("Attack"))
                {
                    Attack();
                }
            }
        }
    }

    private void Attack()  {
        gameObject.GetComponent<PlayerMovement>().animator.SetFloat("xTargeting", Input.GetAxisRaw("Horizontal"));
        gameObject.GetComponent<PlayerMovement>().animator.SetFloat("yTargeting", Input.GetAxisRaw("Vertical"));
        gameObject.GetComponent<PlayerMovement>().animator.SetBool("Attacking", true);
        currentTimeBetween = timeBetweenAttacks;
        if(player.GetComponent<PlayerInventory>().GetMain() == RECIPE.EnergizingDish)
        {
            currentTimeBetween -= energizingAttackModifier;
        }
        Vector3 mousePlayerVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // Get the vector from player to mouse
        if (Input.GetAxisRaw("Vertical") > 0)
        {
                if (player.GetComponent<PlayerInventory>().GetMain() == RECIPE.StrechyDish)
                {
                    Instantiate(StretchyAttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + strechyYOffset), Quaternion.identity, gameObject.transform);
                }
                else
                {
                    Instantiate(AttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yOffset), Quaternion.identity, gameObject.transform);
                }
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (player.GetComponent<PlayerInventory>().GetMain() == RECIPE.StrechyDish)
            {
                Instantiate(StretchyAttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - strechyYOffset), Quaternion.identity, gameObject.transform);
            }
            else
            {
                Instantiate(AttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - yOffset), Quaternion.identity, gameObject.transform);
            }
        }
        else if (gameObject.GetComponent<PlayerMovement>().isFacingRight)
        {
            if (player.GetComponent<PlayerInventory>().GetMain() == RECIPE.StrechyDish)
            {
                Instantiate(StretchyAttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x + strechyXOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
            }
            else
            {
                Instantiate(AttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x + xOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
            }
        }
        else
        {
            if (player.GetComponent<PlayerInventory>().GetMain() == RECIPE.StrechyDish)
            {
                Instantiate(StretchyAttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x - strechyXOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
            }
            else
            {
                Instantiate(AttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x - xOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
            }
        }
    }

    public void LockAttack()
    {
        attackLocked = true;
    }

    public void UnlockAttack()
    {
        attackLocked = false;
    }
}
