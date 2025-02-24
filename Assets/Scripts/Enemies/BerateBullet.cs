using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerateBullet : MonoBehaviour
{
    public float fireDelay = 0.5f;
    public float speed = 3.0f;
    GameObject player;
    bool fire = false;
    bool fired = false;
    public float lifeSpan = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        fireDelay -= Time.deltaTime;
        if(fireDelay <= 0)
        {
            fire = true;
        }
        if (fire && !fired)
        {
            fired = true;

            Vector2 direction = player.transform.position - gameObject.transform.position;
            direction.Normalize();
            gameObject.GetComponent<Rigidbody2D>().velocity = speed * direction;
        }

        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
