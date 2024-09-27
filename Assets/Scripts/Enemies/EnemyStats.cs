using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 1;
    public int maxHealth = 1;
    public int defense = 0;
    bool invuln = false;
    public bool basicHitBox = true;
    float invulnTimer;

    // Update is called once per frame
    void Update()
    {
        invulnTimer -= Time.deltaTime;
        if (invulnTimer <= 0)
        {
            invuln = false;
        }
    }

    public void TakeDamage( int damage)
    {
        if (!invuln)
        {
            health -= damage - defense;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ActivateInvuln(float invulnDuration)
    {
        invuln = true;
        invulnTimer = invulnDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (basicHitBox)
        {
            if (collision.gameObject.tag == "PlayerAttack")
            {
                TakeDamage(collision.GetComponent<PlayerAttack>().damage);
            }
        }
    }
}
