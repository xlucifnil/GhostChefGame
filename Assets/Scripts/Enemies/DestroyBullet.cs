using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    int count = 0;

    // Update is called once per frame
    void Update()
    {
        count++;

        if (count == 5000)
            Destroy(gameObject);
    }
}
