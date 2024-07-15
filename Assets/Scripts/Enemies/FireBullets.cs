using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
    [SerializeField] GameObject BulletBlock;
    [SerializeField] GameObject BulletNoBlock;
    int count = 0;
    GameObject bullet;

    private void Update()
    {
        count++;
        if (count % 3000 == 0)
            Shoot();
    }

    private void Shoot()
    {
        int rand = Random.Range(1, 5);
        if (rand == 4)
        {
            bullet = Instantiate(BulletBlock, new Vector3(transform.position.x + 1, transform.position.y, 0), Quaternion.identity);
        }
        else
        {
            bullet = Instantiate(BulletNoBlock, new Vector3(transform.position.x + 1, transform.position.y, 0), Quaternion.identity);
        }
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(3, 0);
    }
}
