using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] GameObject AttackHitBoxHorizontal; // Default faces right
    [SerializeField] GameObject AttackHitBoxVertical;
    public float yOffset = 0;
    public float xOffset = 0;
    public float timeBetweenAttacks = 1;
    float currentTimeBetween = 0;

    private void Update()
    {
        currentTimeBetween -= Time.deltaTime;
        if (currentTimeBetween <= 0)
        {
            if (Input.GetMouseButtonDown(0))
                Attack();
        }
    }

    private void Attack()
    {
        currentTimeBetween = timeBetweenAttacks;
        Vector3 mousePlayerVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // Get the vector from player to mouse

        if (Mathf.Abs(mousePlayerVector.x) > Mathf.Abs(mousePlayerVector.y)) // If mouse is more Left/Right than Up/Down
        {
            if (mousePlayerVector.x >= 0) // Right
            {
                Instantiate(AttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x + xOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
            }
            else // Left
            {
                Instantiate(AttackHitBoxHorizontal, new Vector3(gameObject.transform.position.x - xOffset, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
            }
        }
        else // If mouse is more Up/Down than Left/Right
        {
            if (mousePlayerVector.y >= 0) // Up
            {
                Instantiate(AttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yOffset), Quaternion.identity, gameObject.transform);
            }
            else // Down
            {
                Instantiate(AttackHitBoxVertical, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - yOffset), Quaternion.identity, gameObject.transform);
            }
        }
    }
}
