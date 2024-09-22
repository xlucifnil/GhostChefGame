using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlappingFlight : MonoBehaviour
{
    public GameObject midPoint;
    public float minHeight = 1.0f;
    public float flapPower;
    public float flapTime = 1f;
    float currentFlapTime = 0f;

    // Update is called once per frame
    void Update()
    {
        currentFlapTime -= Time.deltaTime;

        if(gameObject.transform.localPosition.y <= minHeight && currentFlapTime <= 0f)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity.Set(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * flapPower);
            currentFlapTime = flapTime;
        }
    }
}
