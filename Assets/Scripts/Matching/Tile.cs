using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool selected = false;
    public bool matched = false;
    public string type;
    public int x, y;
    public Vector3 position;
    public GameObject destroyEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MatchEffect()
    {
        GameObject explosion = Instantiate(destroyEffect);
        explosion.transform.position = transform.position;
    }

    public void SetPosition(int newX, int newY, Vector3 newPosition)
    {
        x = newX;
        y = newY;
        position = newPosition;
    }
}
