using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int maxHealth = 6;
    public static int health = 6;
    public static int defense = 0;
    public int numSnacks = 3;
    public int maxSnacks = 3;
    public int snackHealing = 2;
    public static float maxEnergy = 100;
    public static float currentEnergy = 50;
    public float invulnDuration = 1;
    public float thinBrothInvulnBonus = 1;
    public float refreshingSetTime = 5f;
    public float refreshingCurrentTime;
    public GameObject damageBurst;
    float invulnTime;
    public bool invulnerable = false;
    public Color invulnColor = Color.white;
    public Color refreshingColor = Color.blue;
    Color originalColor = Color.white;
    public GameObject playerUI;

    private void Start()
    {
        refreshingCurrentTime = refreshingSetTime;
        originalColor = gameObject.GetComponent<SpriteRenderer>().color;
        playerUI = Instantiate(playerUI);
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
                gameObject.GetComponent<SpriteRenderer>().color = originalColor;
            }
        }

        if(gameObject.GetComponent<PlayerInventory>().GetDrink() == RECIPE.Refreshing)
        {
            refreshingCurrentTime -= Time.deltaTime;
            if(refreshingCurrentTime <= 0)
            {
                gameObject.GetComponent<SpriteRenderer>().color = refreshingColor;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (refreshingCurrentTime <= 0)
        {
            refreshingCurrentTime = refreshingSetTime;
            invulnTime = invulnDuration;
        }
        else
        {
            refreshingCurrentTime = refreshingSetTime;
            health -= damage - defense;

            invulnTime = invulnDuration;
        }
        
        if(gameObject.GetComponent<PlayerInventory>().GetDrink() == RECIPE.ThinBroth)
        {
            invulnTime += thinBrothInvulnBonus;
        }
        if (gameObject.GetComponent<PlayerInventory>().GetDrink() == RECIPE.StingingBubbly)
        {
            Instantiate(damageBurst, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        }
        if (gameObject.GetComponent<PlayerInventory>().GetDessert() == RECIPE.EmergencyCandy)
        {
            FindAnyObjectByType(typeof(PlayerMovement)).GetComponent<PlayerMovement>().TurnOnEmergencySpeed();
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
            gameObject.GetComponent<SpriteRenderer>().color = invulnColor;
        }
        if (collision.gameObject.tag == "Enemy" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            gameObject.GetComponent<SpriteRenderer>().color = invulnColor;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            gameObject.GetComponent<SpriteRenderer>().color = invulnColor;
        }
        if (collision.gameObject.tag == "Enemy" && !invulnerable)
        {
            TakeDamage(collision.GetComponent<EnemyAttack>().damage);
            invulnerable = true;
            gameObject.GetComponent<SpriteRenderer>().color = invulnColor;
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

    public void FullRestore()
    {
        health = maxHealth;
        playerUI.GetComponent<PlayerUI>().DisplayHealth();
        currentEnergy = maxEnergy;
        playerUI.GetComponent<PlayerUI>().DisplayEnergy();
        numSnacks = maxSnacks;
        playerUI.GetComponent<PlayerUI>().SnackText.text = numSnacks.ToString();
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return health;
    }
    
    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }
    
    public float GetMaxEnergy()
    {
        return maxEnergy;
    }
}
