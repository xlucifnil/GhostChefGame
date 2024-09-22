using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoleAI : MonoBehaviour
{
    public enum States
    {
        Waiting,
        Jumping,
        Falling
    }

    Vector2 startPosition;
    public float jumpPower;
    public float jumpDelay;
    public float fallSpeed;
    public float detectionRange;
    public GameObject player;
    public States currentState = States.Waiting;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case States.Waiting:
                if (player.transform.position.y >= transform.position.y)
                {
                    if (player.transform.position.x >= transform.position.x - detectionRange && player.transform.position.x <= transform.position.x + detectionRange)
                    {
                        currentState = States.Jumping;
                    }
                }

                break;

            case States.Jumping:
                currentState = States.Falling;
                gameObject.GetComponent<Rigidbody2D>().velocity.Set(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower);
                gameObject.GetComponent<Rigidbody2D>().gravityScale = fallSpeed;

                break;

            case States.Falling:
                if (transform.position.y <= startPosition.y)
                {
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    transform.position = startPosition;
                    currentState = States.Waiting;
                }

                break;
        }
    }
}
