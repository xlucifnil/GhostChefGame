using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteHitbox : MonoBehaviour
{
    public int AttackDuration; //Ticks?
    int count = 0;

    // Update is called once per frame
    void Update()
    {
        count++;

        if (count == AttackDuration)
            Destroy(gameObject);
    }
}
