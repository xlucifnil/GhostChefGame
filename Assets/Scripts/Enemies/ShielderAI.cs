using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderAI : MonoBehaviour
{
    public enum States
    {
        Idle,
        Attack,
        Prepping,
        Guard
    }

    public States currentState = States.Idle;
    public GameObject shieldLeft, shieldRight, shieldUp;
    public GameObject swordLeft, swordRight, swordUp;
    public float guardTriggerHeight = 0.5f;
    public float guardDelay;
    float currentGuardTime;
    public float attackTime;
    float currentAttackTime;
    public float attackDuration;
    float currentAttackDuration;
    public float cooldown;
    float currentCooldown;
    public float counterTime;
    float currentcounterTime;
    bool counter = false;
    bool swung = false;
    public float shieldInvulnTime = 0.01f;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentGuardTime = guardDelay;
        currentAttackTime = attackTime;
        currentAttackDuration = attackDuration;
        currentCooldown = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.Idle:
                swordUp.SetActive(false);
                swordRight.SetActive(false);
                swordLeft.SetActive(false);
                currentCooldown -= Time.deltaTime;
                if(currentCooldown <= 0)
                {
                    currentCooldown = cooldown;
                    currentState = States.Guard;
                }
                break;

            case States.Prepping:
                shieldUp.SetActive(false);
                shieldRight.SetActive(false);
                shieldLeft.SetActive(false);
                currentAttackTime -= attackTime;

                if(currentAttackTime <= 0)
                {
                    currentState = States.Attack;
                    currentAttackTime = attackTime;
                }

                break;

            case States.Attack:
                if (!swung)
                {
                    if (player.transform.position.y > gameObject.transform.position.y + guardTriggerHeight)
                    {
                        swordUp.SetActive(true);
                    }
                    else if (player.transform.position.x > gameObject.transform.position.x)
                    {
                        swordRight.SetActive(true);
                    }
                    else
                    {
                        swordLeft.SetActive(true);
                    }
                    swung = true;
                    currentAttackDuration = attackDuration;
                }
                else
                {
                    currentAttackDuration -= Time.deltaTime;
                    if(currentAttackDuration <= 0)
                    {
                        swordLeft.SetActive(false);
                        swordRight.SetActive(false);
                        swordUp.SetActive(false);
                        currentState = States.Idle;
                        swung = false;
                    }
                }
                break;

            case States.Guard:
                currentGuardTime -= Time.deltaTime;
                if (player.transform.position.y > gameObject.transform.position.y + guardTriggerHeight)
                {
                    if (shieldUp.activeSelf == false && currentGuardTime <= 0)
                    {
                        shieldUp.SetActive(true);
                        shieldRight.SetActive(false);
                        shieldLeft.SetActive(false);
                        currentGuardTime = guardDelay;
                    }
                }
                else if (player.transform.position.x > gameObject.transform.position.x)
                {
                    if (shieldRight.activeSelf == false && currentGuardTime <= 0)
                    {
                        shieldUp.SetActive(false);
                        shieldRight.SetActive(true);
                        shieldLeft.SetActive(false);
                        currentGuardTime = guardDelay;
                    }
                }
                else
                {
                    if (shieldLeft.activeSelf == false && currentGuardTime <= 0)
                    {
                        shieldUp.SetActive(false);
                        shieldRight.SetActive(false);
                        shieldLeft.SetActive(true);
                        currentGuardTime = guardDelay;
                    }
                }
                currentcounterTime -= Time.deltaTime;
                if(currentcounterTime <= 0 && counter)
                {
                    currentState = States.Prepping;
                    counter = false;
                }
                
                break;
        }
    }

    public void CounterAttack()
    {
        counter = true;
        currentcounterTime = counterTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState != States.Guard)
        {
            if (collision.gameObject.tag == "PlayerAttack")
            {
                GetComponent<EnemyStats>().TakeDamage(collision.GetComponent<PlayerAttack>().damage);
            }
        }
        else
        {
            bool invuln = false;
            if(shieldLeft.activeSelf && player.gameObject.transform.position.x < shieldLeft.transform.position.x)
            {
                invuln = true; 
            }
            else if (shieldRight.activeSelf && player.gameObject.transform.position.x > shieldRight.transform.position.x)
            {
                invuln = true;
            }
            else if (shieldUp.activeSelf && player.gameObject.transform.position.y > shieldUp.transform.position.y)
            {
                invuln = true;
            }
            if (!invuln)
            {
                GetComponent<EnemyStats>().TakeDamage(collision.GetComponent<PlayerAttack>().damage);
            }
        }
    }
}
