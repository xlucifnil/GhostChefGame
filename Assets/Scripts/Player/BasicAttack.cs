using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        currentTimeBetween -= Time.deltaTime;
        if (Time.timeScale != 0f)
        {
            if (currentTimeBetween <= 0)
            {
                if (Input.GetMouseButtonDown(0))
                    Attack();
            }
        }
    }

    private void Attack()
    {
        currentTimeBetween = timeBetweenAttacks;
        if(player.GetComponent<PlayerInventory>().Main.name == RECIPE.EnergizingDish)
        {
            currentTimeBetween -= energizingAttackModifier;
        }
        Vector3 mousePlayerVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // Get the vector from player to mouse

        if (Mathf.Abs(mousePlayerVector.x) > Mathf.Abs(mousePlayerVector.y)) // If mouse is more Left/Right than Up/Down
        {
            if (mousePlayerVector.x <= 0) // Right
            {
                if (player.GetComponent<PlayerInventory>().Main.name == RECIPE.StrechyDish)
                {
                    Instantiate(StretchyAttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x + strechyXOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
                }
                else
                {
                    Instantiate(AttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x + xOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
                }
            }
            else // Left
            {
                if (player.GetComponent<PlayerInventory>().Main.name == RECIPE.StrechyDish)
                {
                    Instantiate(StretchyAttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x - strechyXOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
                }
                else
                {
                    Instantiate(AttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x - xOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
                }
            }
        }
        else // If mouse is more Up/Down than Left/Right
        {
            if (mousePlayerVector.y >= 0) // Up
            {
                if (player.GetComponent<PlayerInventory>().Main.name == RECIPE.StrechyDish)
                {
                    Instantiate(StretchyAttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + strechyYOffset), Quaternion.identity, gameObject.transform);
                }
                else
                {
                    Instantiate(AttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yOffset), Quaternion.identity, gameObject.transform);
                }
                
            }
            else // Down
            {
                if (player.GetComponent<PlayerInventory>().Main.name == RECIPE.StrechyDish)
                {
                    Debug.Log("Strechy");
                    Instantiate(StretchyAttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - strechyYOffset), Quaternion.identity, gameObject.transform);
                }
                else
                {
                    Instantiate(AttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - yOffset), Quaternion.identity, gameObject.transform);
                }
            }
        }
    }
}
