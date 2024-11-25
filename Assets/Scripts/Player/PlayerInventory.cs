using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MEALTYPE
{
    Drink,
    Main,
    Side,
    Dessert
}

public enum RECIPE
{
    Martini,
    Soda,
    MilkShake,
    Burger,
    Pizza,
    Omelett,
    Fries,
    MozzSticks,
    Toast,
    Pie,
    Cannoli,
    Muffin
}

public class PlayerInventory : MonoBehaviour
{
    public struct FoodSlot
    {
        public RECIPE name;
        public MEALTYPE foodType;
        public Sprite foodImage;
    }

    public Dictionary<RECIPE, bool> recipeList = new Dictionary<RECIPE, bool> { };

    //Meal Slots
    public FoodSlot Drink, Main, Side, Dessert;
    //Recipes
    //Drink
    [Header("Drinks")]
    public bool martini;
    public bool soda, milkShake;
    //Main
    [Header("Mains")]
    public bool burger;
    public bool pizza, Omelett;
    //Side
    [Header("Sides")]
    public bool fries;
    public bool mozzSticks, toast;
    //Dessert
    [Header("Desserts")]
    public bool pie;
    public bool cannoli, muffin;
    //Upgrades

    //KeyItems

    public void SwapDrink(RECIPE name, Sprite foodSprite)
    {
        Drink.name = name;
        Drink.foodImage = foodSprite;
    }

    public void SwapMain(RECIPE name, Sprite foodSprite)
    {
        Main.name = name;
        Main.foodImage = foodSprite;
    }

    public void SwapSide(RECIPE name, Sprite foodSprite)
    {
        Side.name = name;
        Side.foodImage = foodSprite;
    }

    public void SwapDessert(RECIPE name, Sprite foodSprite)
    {
        Dessert.name = name;
        Dessert.foodImage = foodSprite;
    }

    public void MakeRecipeList()
    {
        recipeList.Clear();
        
        recipeList.Add(RECIPE.Martini, martini);
        recipeList.Add(RECIPE.Soda, soda);
        recipeList.Add(RECIPE.MilkShake, milkShake);
        recipeList.Add(RECIPE.Burger, burger);
        recipeList.Add(RECIPE.Pizza, pizza);
        recipeList.Add(RECIPE.Omelett, Omelett);
        recipeList.Add(RECIPE.Fries, fries);
        recipeList.Add(RECIPE.MozzSticks, mozzSticks);
        recipeList.Add(RECIPE.Toast, toast);
        recipeList.Add(RECIPE.Pie, pie);
        recipeList.Add(RECIPE.Cannoli, cannoli);
        recipeList.Add(RECIPE.Muffin, muffin);
    }
}