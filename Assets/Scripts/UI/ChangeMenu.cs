using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    public GameObject[] menus;

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
}
