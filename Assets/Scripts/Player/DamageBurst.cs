using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBurst : MonoBehaviour
{
    public float growSpeed;
    public float lifeTime;  
    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0 )
        {
            Destroy(gameObject);
        }
        gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x + growSpeed * Time.deltaTime, gameObject.transform.localScale.y + growSpeed * Time.deltaTime);
    }
}
