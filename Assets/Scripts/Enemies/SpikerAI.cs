using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikerAI : MonoBehaviour
{
    public enum State
    {
        Wander,
        Guard,
        Attack
    }

    public State currentState = State.Wander;

    public bool facingRight = true;
    public float moveSpeed = 1.0f;
    public float guardTime = 1.0f;
    float currentGuardTime;
    public float attackPause = 2.0f;
    float currentAttackPause;
    public float guardDistance = 10f;
    public float spikeSpeed = 1f;
    public GameObject spike;
    public GameObject[] spikeSpawners;
    List<GameObject> spikes;
    GameObject player;

    Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myRigidbody = GetComponent<Rigidbody2D>();
        currentGuardTime = guardTime;
        currentAttackPause = attackPause;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Wander:
                if (facingRight)
                {
                    myRigidbody.velocity = transform.right * moveSpeed;
                }
                else
                {
                    myRigidbody.velocity = -transform.right * moveSpeed;
                }

                if(Vector2.Distance(gameObject.transform.position, player.transform.position) < guardDistance)
                {
                    currentState = State.Guard;
                    myRigidbody.velocity = myRigidbody.velocity*0f;
                }
                break;

            case State.Guard:
                if (player != null)
                {
                    if (Vector2.Distance(gameObject.transform.position, player.transform.position) > guardDistance && currentGuardTime <= 0)
                    {
                        currentState = State.Wander;
                        for (int i = 0; i < spikes.Count; i++)
                        {
                            Destroy(spikes[i]);
                        }
                        spikes.Clear();
                        currentGuardTime = guardTime;

                    }
                    else if (currentGuardTime == guardTime)
                    {
                        spikes = new List<GameObject>();

                        for (int i = 0; i < spikeSpawners.Length; i++)
                        {
                            Debug.Log("Spikes");
                            GameObject aSpike = Instantiate(spike, spikeSpawners[i].transform.position, spikeSpawners[i].transform.rotation);
                            spikes.Add(aSpike);
                            //spikes[i].name = "Spike" + i;

                        }
                        currentGuardTime -= Time.deltaTime;
                    }
                    else if (Vector2.Distance(gameObject.transform.position, player.transform.position) > guardDistance)
                    {
                        currentGuardTime -= Time.deltaTime;
                    }
                }
                break;

            case State.Attack:
                if (currentAttackPause <= 0)
                {
                    currentState = State.Wander;
                    currentAttackPause = attackPause;
                }
                else
                {
                    currentAttackPause -= Time.deltaTime;
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if(facingRight)
            {
                facingRight = false;
            }
            else
            {
                facingRight = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack" && currentState == State.Guard)
        {
            for (int i = 0; i < spikes.Count; i++)
            {
                spikes[i].GetComponent<SpikerSpike>().shooting = true;
                spikes[i].GetComponent<Rigidbody2D>().velocity = (spikes[i].transform.up) * spikeSpeed;
            }
            spikes.Clear();
            currentState = State.Attack;
            currentGuardTime = guardTime;
        }
        else if(collision.gameObject.tag == "PlayerAttack")
        {
            gameObject.GetComponent<EnemyStats>().TakeDamage(collision.GetComponent<PlayerAttack>().damage);
        }
    }
}
