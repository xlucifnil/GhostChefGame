using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 5;
    public int health = 5;
    public int defense = 0;
    public int numSnacks = 3;
    public int maxSnacks = 3;
    public int snackHealing = 2;
    public float maxEnergy = 100;
    public float currentEnergy = 50;
    public float invulnDuration = 1;
    public GameObject damageBurst;
    float invulnTime;
    public bool invulnerable = false;
    public Color invulnColor = Color.white;
    Color originalColor = Color.white;
    GameObject playerUI;

    private void Start()
    {
        invulnTime = invulnDuration;
        originalColor = gameObject.transform.parent.GetComponent<SpriteRenderer>().color;
        playerUI = GameObject.Find("PlayerUI");
        playerUI.GetComponent<PlayerUI>().SnackText.text = numSnacks.ToString();
    }

    private void Update()
    {
        if (invulnerable)
        {
            invulnTime -= Time.deltaTime;

            if (invulnTime <= 0)
            {
                invulnTime = invulnDuration;
                invulnerable = false;
                gameObject.transform.parent.GetComponent<SpriteRenderer>().color = originalColor;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {

        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage - defense;

        if (gameObject.GetComponent<PlayerInventory>().Drink.name == RECIPE.StingingBubbly)
        {
            Instantiate(damageBurst, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        }
        if (playerUI != null)
        {
            playerUI.GetComponent<PlayerUI>().DisplayHealth();
        }
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            gameObject.transform.parent.GetComponent<SpriteRenderer>().color = invulnColor;
        }
        if (collision.gameObject.tag == "Enemy" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            gameObject.transform.parent.GetComponent<SpriteRenderer>().color = invulnColor;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            gameObject.transform.parent.GetComponent<SpriteRenderer>().color = invulnColor;
        }
        if (collision.gameObject.tag == "Enemy" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            gameObject.transform.parent.GetComponent<SpriteRenderer>().color = invulnColor;
        }
    }

    public void EatSnack()
    {
        numSnacks--;
        playerUI.GetComponent<PlayerUI>().SnackText.text = numSnacks.ToString();
        health += snackHealing;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        playerUI.GetComponent<PlayerUI>().DisplayHealth();
    }

    public void SpendEnergy(float spent)
    {
        currentEnergy -= spent;
        playerUI.GetComponent<PlayerUI>().DisplayEnergy();
    }
}
