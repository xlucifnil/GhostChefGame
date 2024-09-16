using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicFlyerAI : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase
    }
    public State state = State.Patrol;
    public GameObject[] path;
    int currentTarget = 0;
    public float patrolSpeed = 1.0f;
    public float chaseDistance = 10.0f;
    public float chaseSpeed = 1.0f;
    public float escapeDistance = 11.0f;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Patrol:
                if (path.Length > 0)
                {
                    gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, path[currentTarget].transform.position, Time.deltaTime * patrolSpeed);

                    if (gameObject.transform.position == path[currentTarget].transform.position)
                    {
                        currentTarget++;
                        if (currentTarget >= path.Length)
                        {
                            currentTarget = 0;
                        }
                    }
                }
                if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= chaseDistance)
                {
                    state = State.Chase;
                }
                break;

            case State.Chase:
                if (player != null)
                {
                    if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= escapeDistance)
                    {
                        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position, Time.deltaTime * chaseSpeed);
                    }
                    else
                    {
                        state = State.Patrol;
                    }
                }
                break;
        }

        
    }
}
