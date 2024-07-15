using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string DoorCode;
    public string SceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager.PlayerCurrentDoorCode = DoorCode;
        SceneManager.LoadScene(SceneToLoad);
    }
}
