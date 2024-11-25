using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMenu : MonoBehaviour
{
    public GameObject[] menus;
    public GameObject[] mealMenus;
    public GameObject drinkImage;
    public GameObject mainImage;
    public GameObject sideImage;
    public GameObject dessertImage;
    GameObject player;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInventory>().MakeRecipeList();
        if(player.GetComponent<PlayerInventory>().Drink.foodImage != null)
         drinkImage.GetComponent<Image>().sprite = player.GetComponent<PlayerInventory>().Drink.foodImage;

        if (player.GetComponent<PlayerInventory>().Main.foodImage != null)
            mainImage.GetComponent<Image>().sprite = player.GetComponent<PlayerInventory>().Main.foodImage;

        if (player.GetComponent<PlayerInventory>().Side.foodImage != null)
            sideImage.GetComponent<Image>().sprite = player.GetComponent<PlayerInventory>().Side.foodImage;

        if (player.GetComponent<PlayerInventory>().Dessert.foodImage != null)
            dessertImage.GetComponent<Image>().sprite = player.GetComponent<PlayerInventory>().Dessert.foodImage;
    }
    public void SwitchMenu(GameObject openMenu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
        openMenu.SetActive(true);
    }

    public void CloseMenu(GameObject menuHolder)
    {
        Destroy(menuHolder);
    }

    public void SwitchMealMenu(GameObject openMenu)
    {
        for (int i = 0; i < mealMenus.Length; i++)
        {
            mealMenus[i].SetActive(false);
        }
        openMenu.SetActive(true);
    }
}
