using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool addRecipe = false;
    public bool addItem = false;
    public RECIPE recipe;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (addRecipe)
            {
                collision.gameObject.GetComponent<PlayerInventory>().AddRecipeToInventory(recipe);
            }

            if (addItem)
            { 
                
            }
            
            Destroy(gameObject);
        }
    }
}
