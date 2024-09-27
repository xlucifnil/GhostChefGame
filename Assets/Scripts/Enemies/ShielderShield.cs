using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ShielderShield : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerAttack")
        {
            gameObject.transform.parent.gameObject.GetComponent<ShielderAI>().CounterAttack();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerAttack")
        {
            gameObject.transform.parent.gameObject.GetComponent<ShielderAI>().CounterAttack();
        }
    }
}
