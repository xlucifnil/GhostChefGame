using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteHitbox : MonoBehaviour
{
    public float AttackDuration;
    float count = 0;

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;

        if (count >= AttackDuration)
            Destroy(gameObject);
    }
}
