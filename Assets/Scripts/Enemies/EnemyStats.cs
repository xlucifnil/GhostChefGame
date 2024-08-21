using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 1;
    public int maxHealth = 1;
    public int defense = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage( int damage)
    {
        health -= damage - defense;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
