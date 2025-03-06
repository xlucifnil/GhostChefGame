using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontal;
    public float speed = 8f;
    public float hyperSpeedModifier = 2f;
    public float emergencySpeedModifier = 2f;
    public float emergencySpeedDuration = 2f;
    float emergencyTimeLeft;
    public float snackSpeedModifier = -4f;
    public float jumpingPower = 20f;
    public float hoverModifier = .2f;
    public float lightHoverModifier = .1f;
    public float snackTime;
    public float ectoTime = 0.5f;
    float tillEctoTime = 0;
    float tillFloorEcto = 0;
    public float floorEctoOffset = 0.5f;
    float currentSnackTime;
    bool snacking = false;
    public GameObject ectoGlideTrail;
    public bool isFacingRight = true;
    private GameObject player;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask calledGroundLayer;

    private void Start()
    {
        currentSnackTime = snackTime;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0f)
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded())
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                    tillEctoTime = 0;
                }
            }

            if (Input.GetButton("Jump") && rb.velocity.y <= 0f)
            {
                if(player.GetComponent<PlayerInventory>().GetSide() == RECIPE.LightSide)
                {
                    rb.gravityScale = lightHoverModifier;
                }
                else
                {
                    rb.gravityScale = hoverModifier;
                }
                
                if(player.GetComponent<PlayerInventory>().GetSide() == RECIPE.Ectomash)
                {
                    tillEctoTime -= Time.deltaTime;
                    if (tillEctoTime < 0 && !IsGrounded())
                    {
                        tillEctoTime = ectoTime;
                        Instantiate(ectoGlideTrail,gameObject.transform.position, gameObject.transform.rotation);
                    }
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                if (rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                }
                rb.gravityScale = 4;
            }

            if(IsGrounded() && rb.velocity.x != 0.0f && player.GetComponent<PlayerInventory>().GetDessert() == RECIPE.Ectojello)
            {
                tillFloorEcto -= Time.deltaTime;
                if(tillFloorEcto <= 0.0f)
                {
                    tillFloorEcto = ectoTime;
                    Instantiate(ectoGlideTrail, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + floorEctoOffset), gameObject.transform.rotation);
                }
            }

            if (IsGrounded() && Input.GetKeyDown(KeyCode.LeftShift) && player.gameObject.GetComponent<PlayerStats>().numSnacks > 0)
            {
                snacking = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                currentSnackTime = snackTime;
                snacking = false;
            }
            else if (!IsGrounded())
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
    }

    private void FixedUpdate()
    {
        if (Time.timeScale != 0f)
        {
            float moveSpeed = speed;

            if(player.GetComponent<PlayerInventory>().GetDessert() == RECIPE.Hyper)
            {
                moveSpeed += hyperSpeedModifier;
            }
            if (snacking)
            {
                moveSpeed += snackSpeedModifier;   
            }
            if(emergencyTimeLeft > 0)
            {
                emergencyTimeLeft -= Time.deltaTime;
                moveSpeed += emergencySpeedModifier;
            }

            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
    }

    public bool IsGrounded()
    {
        bool floored = false;
        floored =  Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (floored)
        {
            return floored;
        }

        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, calledGroundLayer);
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

    public void TurnOnEmergencySpeed()
    {
        emergencyTimeLeft = emergencySpeedDuration;
    }
}
