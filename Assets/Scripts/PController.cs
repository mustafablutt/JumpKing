using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PController     : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Animator anim;

    private float movementInputDirection;
    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;

    public int amountOfJumps = 2 ;
    private int amountOfJumpsLeft;

    private bool isFacingRight;
    private bool isWalking; 
    private bool isGrounded;
    private bool canJump;
    private bool isTouchingWall; 

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGrounded;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    
    void Update()
    {
        CheckInput();
        checkMovementDirection();
        UpdateAnimation();
        checkIfCanJump();
    }
    void FixedUpdate()
    {
        ApplyMovement();
        checkSurroundings();
    } 
   
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
           
                Jump();
        }
    }
    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementSpeed * movementInputDirection,rb.velocity.y);
    }
    private void Jump()
    {
        
        if (canJump)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
            
    }
    private void checkIfCanJump()
    {
       
        if(isGrounded == true && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps; 
        }
        if(amountOfJumpsLeft <= 0)
        {
            canJump = false;

        }
        else
        {
            canJump = true;
        }
    }
   
    private void checkMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
        if(rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    #region Animation
    private void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
    }
    #endregion
    
    
    private void checkSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGrounded);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGrounded);
    }
   

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
