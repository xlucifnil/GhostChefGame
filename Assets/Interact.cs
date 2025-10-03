using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    bool canActivate = false;

    GameObject Player;
    GameObject PlayerParent;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerParent = Player.transform.parent.gameObject;
    }

    private void Update()
    {
        if (canActivate)
        {
            if (Input.GetAxisRaw("Vertical") > 0.5 && Input.GetAxisRaw("Horizontal") < .2)
            {
                canActivate = false;
                PlayerParent.GetComponent<PlayerMovement>().LockMovement();
                PlayerParent.GetComponent<BasicAttack>().LockAttack();
                Debug.Log("Display Dialog");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canActivate = false;
        }
    }
}
