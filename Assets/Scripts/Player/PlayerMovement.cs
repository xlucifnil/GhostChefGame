using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontal;
    public float speed = 8f;
    public float jumpingPower = 20f;
    public float hoverModifier = .2f;
    public float snackSpeed = 4f;
    public float snackTime;
    float currentSnackTime;
    public bool snacking = false;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        currentSnackTime = snackTime;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }
        }

        if(Input.GetButton("Jump") && rb.velocity.y <= 0f)
        {
            rb.gravityScale = hoverModifier;
        }

        //Debug.Log(rb.velocity);

        if (Input.GetButtonUp("Jump"))
        {
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
            rb.gravityScale = 4;
        }

        if(IsGrounded() && Input.GetKeyDown(KeyCode.LeftShift) && gameObject.GetComponent<PlayerStats>().numSnacks > 0)
        {
            snacking = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSnackTime = snackTime;
            snacking = false;
        }
        else if(!IsGrounded())
        {
            currentSnackTime = snackTime;
            snacking = false;
        }

        if (snacking)
        {
            currentSnackTime -= Time.deltaTime;
            if (currentSnackTime <= 0)
            {
                gameObject.GetComponent<PlayerStats>().EatSnack();
                snacking = false;
                currentSnackTime = snackTime;
            }
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if(!snacking)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * snackSpeed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if ( (isFacingRight && horizontal < 0f ||  !isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    
}
