using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [HideInInspector]
    public int playerInputID = 1;
    //[HideInInspector]
    //public int playerID = 1;
    [Space] // space needs to be on top of header for the header to have a space under it.
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public JumpPhysics jumpPhysics;
    [Space]
    [Header("Collisions")]
    public float jumpCheckRadius = 0.3f;
    public float headCheckRadius = 0.4f;
    public float walldetectionHeight = 0.8f;
    public float walldetectionWidth = 0.1f;
    public float jumpVelocity;
    [Space]
    public LayerMask ground;
    public bool Moving
    {
        get { return xInput != 0 && canMove && !isCrouching; }
    }
    bool canMove;

    public bool CanJump
    {
        get { return jumpRequest && jumping; }
        set { jumpRequest = value; jumping = value; }
    }
    [Space]
    public Transform frontPos;
    public Transform feetPos;
    public Transform defaultHeadPos;
    public Transform crouchedHeadPos;
    Transform headPos;

    public GameObject defaultSoftSpot;
    public GameObject crouchSoftSpot;
    public GameObject defaultColliders;
    public GameObject crouchColliders;

    [HideInInspector]
    public string horizontal;
    [HideInInspector]
    public string jump;
    [HideInInspector]
    public string crouch;

    float xInput;
    float jumpTimeLeft;
    int jumpsUsed = 0;

    bool isGrounded;
    [HideInInspector]
    public bool isCrouching;
    bool jumpRequest;
    bool jumping;
    bool fell;

    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        headPos = defaultHeadPos;

        jumpPhysics.CalculateGravity();
        rb.gravityScale = jumpPhysics.gravity / 9.81f;
    }

    void Update()
    {
        CheckCollisions();
        GetInputs();
        Move();
        Animate();

        if (isCrouching) // this section of code is poorly optimized
        {
            headPos = crouchedHeadPos; // could set all this using events in animation, just need to put them in functions first
            crouchSoftSpot.SetActive(true);
            defaultSoftSpot.SetActive(false);
            crouchColliders.SetActive(true);
            defaultColliders.SetActive(false);
        }
        else
        {
            headPos = defaultHeadPos;
            defaultSoftSpot.SetActive(true);
            crouchSoftSpot.SetActive(false);
            defaultColliders.SetActive(true);
            crouchColliders.SetActive(false);
        }

    }

    private void Animate()
    {
        animator.SetBool("Running", xInput != 0);
        animator.SetBool("Crouching", isCrouching);
    }

    void CheckCollisions()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, jumpCheckRadius, ground);
        if (isGrounded)
        {
            jumpsUsed = 0;
            fell = false;
        }
        else if (!isGrounded && !fell && !Input.GetButton(jump))
        {
            if (jumpPhysics.numberOfJumps > 1)
                jumpsUsed++;
            fell = true;
        }
        else if (!fell)
            fell = true;

        canMove = !Physics2D.OverlapArea(frontPos.position + (transform.up * walldetectionHeight / 2) + (transform.right * walldetectionWidth / 2),
            frontPos.position + (-transform.up * walldetectionHeight / 2) + (-transform.right * walldetectionWidth / 2), ground);
    }

    void GetInputs()
    {
        xInput = Input.GetAxisRaw(horizontal);
        if (xInput > 0)
            transform.rotation = Quaternion.identity;
        else if (xInput < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        isCrouching = Input.GetButton(crouch);

        if (Input.GetButton(jump) && jumpsUsed < jumpPhysics.numberOfJumps)
        {
            CanJump = true;
            jumpVelocity = jumpPhysics.jumpVelocity;
        }
        else if (Input.GetButtonUp(jump))
        {
            CanJump = false;
            jumpsUsed++;
            jumpTimeLeft = jumpPhysics.jumpTime;
        }
    }

    void Move()
    {
        if (Moving)
        {
            rb.velocity = new Vector2(moveSpeed * xInput, rb.velocity.y);
        }
        if (CanJump)
        {
            bool headTrauma = Physics2D.OverlapCircle(headPos.position, headCheckRadius, ground);

            if (jumpTimeLeft > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPhysics.jumpVelocity);
                jumpVelocity -= Mathf.Sqrt(jumpVelocity) * 2f * Time.deltaTime;
                jumpTimeLeft -= Time.deltaTime;
            }
            if (jumpTimeLeft <= 0 || headTrauma)
            {
                jumping = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(feetPos.position, jumpCheckRadius);

        Gizmos.color = Color.cyan;
        if (headPos != null)
            Gizmos.DrawWireSphere(headPos.position, headCheckRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(frontPos.position + (transform.up * walldetectionHeight / 2) + (transform.right * walldetectionWidth / 2),
            frontPos.position + (-transform.up * walldetectionHeight / 2) + (-transform.right * walldetectionWidth / 2));
        Gizmos.DrawLine(frontPos.position - (transform.up * walldetectionHeight / 2) + (transform.right * walldetectionWidth / 2),
           frontPos.position - (-transform.up * walldetectionHeight / 2) + (-transform.right * walldetectionWidth / 2));
    }

    [System.Serializable]
    public class JumpPhysics
    {
        [HideInInspector]
        public float gravity;
        [HideInInspector]
        public float jumpVelocity;

        public float minJumpHeight = 1f;
        public float timeToJumpApex = 0.15f;
        public float jumpTime = 0.3f;
        public int numberOfJumps = 2;

        public void CalculateGravity()
        {
            gravity = (2 * minJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        }
    }
}
