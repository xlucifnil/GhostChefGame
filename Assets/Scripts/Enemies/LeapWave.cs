using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class LeapWave : MonoBehaviour
{
    public float lifeSpan = 2.0f;
    private void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0 )
        {
            Destroy( this );
        }
    }
    public void Launch(float speed)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
    }
}
