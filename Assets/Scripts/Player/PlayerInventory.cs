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
    Null,
    StingingBubbly,
    ThinBroth,
    Refreshing,
    Lucky,
    EnergizingDish,
    StrechyDish,
    Ectomash,
    LightSide,
    Toast,
    Ectojello,
    Hyper,
    EmergencyCandy
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
    public static FoodSlot Drink;
    public static FoodSlot Main;
    public static FoodSlot Side;
    public static FoodSlot Dessert;
    //Recipes
    public Sprite nullFood;
    public bool empty;
    //Drink
    [Header("Drinks")]
    public bool stingingBubbly;
    public bool thinBroth, refreshing;
    //Main
    [Header("Mains")]
    public bool lucky;
    public bool energizingDish, strechyDish;
    //Side
    [Header("Sides")]
    public bool ectomash;
    public bool lightSide, toast;
    //Dessert
    [Header("Desserts")]
    public bool ectojello;
    public bool hyper, emergencyCandy;
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

        recipeList.Add(RECIPE.Null, empty);
        recipeList.Add(RECIPE.StingingBubbly, stingingBubbly);
        recipeList.Add(RECIPE.ThinBroth, thinBroth);
        recipeList.Add(RECIPE.Refreshing, refreshing);
        recipeList.Add(RECIPE.Lucky, lucky);
        recipeList.Add(RECIPE.EnergizingDish, energizingDish);
        recipeList.Add(RECIPE.StrechyDish, strechyDish);
        recipeList.Add(RECIPE.Ectomash, ectomash);
        recipeList.Add(RECIPE.LightSide, lightSide);
        recipeList.Add(RECIPE.Toast, toast);
        recipeList.Add(RECIPE.Ectojello, ectojello);
        recipeList.Add(RECIPE.Hyper, hyper);
        recipeList.Add(RECIPE.EmergencyCandy, emergencyCandy);
    }

    public RECIPE GetDrink()
    {
        return Drink.name;
    }

    public RECIPE GetSide()
    {
        return Side.name;
    }

    public RECIPE GetMain()
    {
        return Main.name;
    }

    public RECIPE GetDessert()
    {
        return Dessert.name;
    }

    public Sprite GetDrinkImage()
    {
        return Drink.foodImage;
    }

    public Sprite GetSideImage()
    {
        return Side.foodImage;
    }

    public Sprite GetMainImage()
    {
        return Main.foodImage;
    }

    public Sprite GetDessertImage()
    {
        return Dessert.foodImage;
    }
}