using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontal = 0;
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
    bool hovering = false;
    public float slopeRayXOffset = 1.0f; //This is the offset needed to put the raycast at the edge of the player. Do not change unless player size changes.
    //public float slopeRayYOffset = -.5f; //Do not change unless player size changes.
    public float slopeGroundDistance = .2f;
    public float floatHeight = .2f;
    bool jumping = false;
    public bool camping = false;
    bool lockedMovment = false;

    public Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask calledGroundLayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentSnackTime = snackTime;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0f && !lockedMovment)
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                if (IsGroundedRay())
                {
                    jumping = true;
                    animator.SetBool("isJumping", jumping);
                    rb.gravityScale = 4;
                    rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                    rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                    tillEctoTime = 0;
                }
            }

            if (Input.GetButton("Jump") && rb.velocity.y <= 0f)
            {
                hovering = true;
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
                    if (tillEctoTime < 0 && !IsGroundedRay())
                    {
                        tillEctoTime = ectoTime;
                        Instantiate(ectoGlideTrail,gameObject.transform.position, gameObject.transform.rotation);
                    }
                }
            }
            else
            {
                hovering = false;
            }

            if (Input.GetButtonUp("Jump"))
            {
                if (rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                }
                rb.gravityScale = 4;
            }

            if(IsGroundedRay() && rb.velocity.x != 0.0f && player.GetComponent<PlayerInventory>().GetDessert() == RECIPE.Ectojello)
            {
                tillFloorEcto -= Time.deltaTime;
                if(tillFloorEcto <= 0.0f)
                {
                    tillFloorEcto = ectoTime;
                    Instantiate(ectoGlideTrail, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + floorEctoOffset), gameObject.transform.rotation);
                }
            }

            if (IsGroundedRay() && Input.GetKeyDown(KeyCode.LeftShift) && player.gameObject.GetComponent<PlayerStats>().numSnacks > 0)
            {
                snacking = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                currentSnackTime = snackTime;
                snacking = false;
            }
            else if (!IsGroundedRay())
            {
                currentSnackTime = snackTime;
                snacking = false;
            }
            
            if (snacking)
            {
                currentSnackTime -= Time.deltaTime;
                if (currentSnackTime <= 0)
                {
                    player.GetComponent<PlayerStats>().EatSnack();
                    snacking = false;
                    currentSnackTime = snackTime;
                }
            }
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale != 0f && !lockedMovment)
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

            RaycastHit2D LeftHit = Physics2D.Raycast(new Vector2(transform.position.x - slopeRayXOffset, transform.position.y), Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
            RaycastHit2D RightHit = Physics2D.Raycast(new Vector2(transform.position.x + slopeRayXOffset, transform.position.y), Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

            if (IsGroundedRay())
            {
                rb.gravityScale = 0;
            }
            else if (hovering == false)
            {
                rb.gravityScale = 4;
            }
            
            if (IsGroundedRay() && jumping == false)
            {
                animator.SetBool("isJumping", jumping);
                if (RightHit.point.y > LeftHit.point.y && RightHit.rigidbody != null)
                {
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x, RightHit.point.y + floatHeight);
                }
                else
                {
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x, LeftHit.point.y + floatHeight);
                }
                rb.gravityScale = 0;
                rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
            }
            else
            {
                if (!hovering)
                {
                    rb.gravityScale = 4;
                }
                rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
            }

            if(jumping == true)
            {
                if(rb.velocity.y < 0)
                {
                    jumping = false;
                }
            }
            animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
    }

    public bool IsGrounded()
    {
        bool floored = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (floored)
        {
            return floored;
        }

        return floored;
    }

    public bool IsGroundedRay()
    {
        RaycastHit2D LeftHit = Physics2D.Raycast(new Vector2(transform.position.x - slopeRayXOffset, transform.position.y), Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        RaycastHit2D RightHit = Physics2D.Raycast(new Vector2(transform.position.x + slopeRayXOffset, transform.position.y), Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        bool floored = false;

        if (floored)
        {
            return floored;
        }

        if (LeftHit.distance < slopeGroundDistance && LeftHit.collider != null)
        {
            floored = true;
        }
        else if(RightHit.distance < slopeGroundDistance && RightHit.collider != null)
        {
            floored = true;
        }

        return floored;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetAxisRaw("Vertical") >= 0.5f && camping == false)
        {
            camping = true;
            transform.position = new Vector2 (collision.GetComponent<CampingSpot>().playerSpot.transform.position.x, transform.position.y);
            Debug.Log("camp");
        }

        if(Math.Abs(Input.GetAxisRaw("Horizontal")) >= 0.5f && camping == true)
        {
            camping = false;
            Debug.Log("unCamp");
        }
    }

    public void LockMovement()
    {
        lockedMovment = true;
    }
    public void UnlockMovement()
    {
        lockedMovment = false;
    }
}
