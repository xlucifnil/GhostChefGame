using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpot : MonoBehaviour
{
    public string triggerTag;
    public GameObject triggeredItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == triggerTag)
        {
            triggeredItem.SetActive(true);
        }
    }
}
