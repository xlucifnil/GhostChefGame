using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
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

public enum ITEM
{
    Heart1, Heart2, Heart3, Heart4, Heart5, Heart6, Heart7,
    Float
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
    //Upgrades and Items. Things not needed to be interacted with by the player in a menu.

    public static Dictionary<ITEM, bool> inventory = new Dictionary<ITEM, bool>()
    {
        {ITEM.Heart1, false },
        {ITEM.Heart2, false },
        {ITEM.Heart3, false },
        {ITEM.Heart4, false },
        {ITEM.Heart5, false },
        {ITEM.Heart6, false },
        {ITEM.Heart7, false },
        {ITEM.Float, false },
    };

    public void AddRecipeToInventory(RECIPE recipe)
    {
        recipeList[recipe] = true;
    }

    public void AddItemToInventory(ITEM item)
    {
        inventory[item] = true;
    }

    public void SwapDrink(RECIPE name, Sprite foodSprite)
    {
        if (gameObject.transform.parent.GetComponent<PlayerMovement>().camping == true)
        {
            Drink.name = name;
            Drink.foodImage = foodSprite;
        }
    }

    public void SwapMain(RECIPE name, Sprite foodSprite)
    {
        if (gameObject.transform.parent.GetComponent<PlayerMovement>().camping == true)
        {
            Main.name = name;
            Main.foodImage = foodSprite;
        }
    }

    public void SwapSide(RECIPE name, Sprite foodSprite)
    {
        if (gameObject.transform.parent.GetComponent<PlayerMovement>().camping == true)
        {
            Side.name = name;
            Side.foodImage = foodSprite;
        }
    }

    public void SwapDessert(RECIPE name, Sprite foodSprite)
    {
        if (gameObject.transform.parent.GetComponent<PlayerMovement>().camping == true)
        {
            Dessert.name = name;
            Dessert.foodImage = foodSprite;
        }
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