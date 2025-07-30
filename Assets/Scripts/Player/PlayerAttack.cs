using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 1;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player.GetComponent<PlayerInventory>().GetMain() == RECIPE.Lucky)
        {
            damage = damage * 2;
        }
    }
}
