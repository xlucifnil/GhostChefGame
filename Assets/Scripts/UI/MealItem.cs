using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MealItem : MonoBehaviour
{
    public GameObject mealSlot;

    public MEALTYPE mealType;
    public RECIPE recipe;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player.GetComponent<PlayerInventory>().recipeList[recipe] == false)
        {
            gameObject.SetActive(false);
        }
    }

    //public void Awake()
    //{
    //    if (player.GetComponent<PlayerInventory>().recipeList[recipe] == false)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}

    public void SelectMeal()
    {
        mealSlot.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;

        switch(mealType)
        {
            case MEALTYPE.Drink:
                player.GetComponent<PlayerInventory>().SwapDrink(recipe, gameObject.GetComponent<Image>().sprite);
                break;

            case MEALTYPE.Main:
                player.GetComponent<PlayerInventory>().SwapMain(recipe, gameObject.GetComponent<Image>().sprite);
                break;

            case MEALTYPE.Side:
                player.GetComponent<PlayerInventory>().SwapSide(recipe, gameObject.GetComponent<Image>().sprite);
                break;

            case MEALTYPE.Dessert:
                player.GetComponent<PlayerInventory>().SwapDessert(recipe, gameObject.GetComponent<Image>().sprite);
                break;
        }
    }
}
