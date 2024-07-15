using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] GameObject AttackHitBox; // Default faces right

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Attack();
    }

    private void Attack()
    {
        Vector3 mousePlayerVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // Get the vector from player to mouse

        if (Mathf.Abs(mousePlayerVector.x) > Mathf.Abs(mousePlayerVector.y)) // If mouse is more Left/Right than Up/Down
        {
            if (mousePlayerVector.x >= 0) // Right
            {
                Instantiate(AttackHitBox, new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
            }
            else // Left
            {
                Instantiate(AttackHitBox, new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
            }
        }
        else // If mouse is more Up/Down than Left/Right
        {
            if (mousePlayerVector.y >= 0) // Up
            {
                Instantiate(AttackHitBox, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1), Quaternion.identity, gameObject.transform);
            }
            else // Down
            {
                Instantiate(AttackHitBox, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1), Quaternion.identity, gameObject.transform);
            }
        }
    }
}
