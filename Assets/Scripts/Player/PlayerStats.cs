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
    public float invulnDuration = 1;
    float invulnTime;
    public bool invulnerable = false;
    public Color invulnColor = Color.white;
    Color originalColor = Color.white;
    GameObject playerUI;

    private void Start()
    {
        invulnTime = invulnDuration;
        originalColor = GetComponent<SpriteRenderer>().color;
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
                GetComponent<SpriteRenderer>().color = originalColor;
            }
        }

        if(Input.GetMouseButtonDown(1))
        {

        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage - defense;
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
        if(collision.gameObject.tag == "EnemyAttack" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            GetComponent<SpriteRenderer>().color = invulnColor;
        }
        if (collision.gameObject.tag == "Enemy" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            GetComponent<SpriteRenderer>().color = invulnColor;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            GetComponent<SpriteRenderer>().color = invulnColor;
        }
        if (collision.gameObject.tag == "Enemy" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            GetComponent<SpriteRenderer>().color = invulnColor;
        }
    }

    public void EatSnack()
    {
        numSnacks--;
        playerUI.GetComponent<PlayerUI>().SnackText.text = numSnacks.ToString();
        health += snackHealing;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        playerUI.GetComponent<PlayerUI>().DisplayHealth();
    }
}
