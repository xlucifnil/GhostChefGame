using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] List<Door> doors = new List<Door>();
    [SerializeField] GameObject playerObject;

    private void Awake()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i].DoorCode == PlayerManager.PlayerCurrentDoorCode)
            {
                playerObject.transform.position = doors[i].transform.GetChild(0).gameObject.transform.position;
                break;
            }
        }
    }
}
