using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlappingFlight : MonoBehaviour
{
    public GameObject midPoint;
    public float minHeight = 1.0f;
    public float flapPower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.localPosition.y <= minHeight)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * flapPower);
        }

        
    }
}
