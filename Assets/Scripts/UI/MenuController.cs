using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject playerUI;
    public InputAction MenuAction;

    private void Start()
    {
        playerUI = GameObject.Find("PlayerUI");
    }
    void Update()
    {
        MenuToggle();
    }

    public void MenuToggle()
    {
        
        if (MenuAction.IsPressed())
        {
            if (GameObject.Find(menu.name) != null)
            {
                Destroy(GameObject.Find(menu.name));
                playerUI.SetActive(true);
                Time.timeScale = 1.0f;
            }
            else
            {
                GameObject newMenu = Instantiate(menu);
                newMenu.name = menu.name;
                playerUI.SetActive(false);
                Time.timeScale = 0.0f;
            }
        }

        
    }
}
