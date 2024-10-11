using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpot : MonoBehaviour
{
    public string triggerTag;
    public GameObject triggeredItem;
    public bool oneUse;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == triggerTag)
        {
            triggeredItem.SetActive(true);
            if (oneUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
