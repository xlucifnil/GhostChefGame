using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikerSpike : MonoBehaviour
{
    public float life = 10f;
    public bool shooting = false;

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            if(life <= 0)
            {
                Destroy(gameObject);
            }
            life -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shooting)
        {
            Destroy(gameObject);
        }
    }
}
