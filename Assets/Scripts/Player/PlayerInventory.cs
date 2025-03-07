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

    public static Dictionary<RECIPE, bool> recipeList = new Dictionary<RECIPE, bool>()
    {
        {RECIPE.StingingBubbly, false },
        {RECIPE.ThinBroth, false },
        {RECIPE.Refreshing, false },
        {RECIPE.Lucky, false},
        {RECIPE.EnergizingDish, false },
        {RECIPE.StrechyDish, false },
        {RECIPE.Ectomash, false },
        {RECIPE.LightSide, false },
        {RECIPE.Toast, false },
        {RECIPE.Ectojello, false },
        {RECIPE.Hyper, false },
        {RECIPE.EmergencyCandy, false }
    };

    //Meal Slots
    public static FoodSlot Drink;
    public static FoodSlot Main;
    public static FoodSlot Side;
    public static FoodSlot Dessert;
    //Recipes
    public Sprite nullFood;
    //Drink
    public static bool stingingBubbly;
    public static bool thinBroth, refreshing;
    //Main
    public static bool lucky;
    public static bool energizingDish, strechyDish;
    //Side
    public static bool ectomash;
    public static bool lightSide, toast;
    //Dessert
    public static bool ectojello;
    public static bool hyper, emergencyCandy;
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

    public bool RecipeKnown(RECIPE recipe)
    {
        return recipeList[recipe];
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