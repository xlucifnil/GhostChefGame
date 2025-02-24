using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RisingLand : MonoBehaviour
{
    public bool riser = true;
    public float maxHeight;
    float endHeight;
    Vector2 endLocation;
    public float riseSpeed;
    // Start is called before the first frame update
    void Start()
    {
        endHeight = Random.Range(0, maxHeight);
        endLocation = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y + endHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (riser)
        {
            transform.position = Vector2.MoveTowards(transform.position, endLocation, riseSpeed * Time.deltaTime);
            if (transform.position.y > endLocation.y)
            {
                transform.position = endLocation;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyAttack")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyAttack")
        {
            Destroy(gameObject);
        }
    }
}
