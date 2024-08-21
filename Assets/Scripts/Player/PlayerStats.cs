using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 5;
    public int health = 5;
    public int defense = 0;

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
        if(collision.gameObject.tag == "EnemyAttack")
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
        }
    }
}
