using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 5;
    public int health = 5;
    public int defense = 0;
    public float invulnDuration = 1;
    float invulnTime;
    public bool invulnerable = false;
    public Color invulnColor = Color.white;
    Color originalColor = Color.white;

    private void Start()
    {
        invulnTime = invulnDuration;
        originalColor = GetComponent<SpriteRenderer>().color;
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
    }

    public void TakeDamage(int damage)
    {
        health -= damage - defense;

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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            GetComponent<SpriteRenderer>().color = invulnColor;
        }
    }
}
