using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovements : MonoBehaviour
{
    [SerializeField] private float OriginalSpeed=5f;
    private float speed = 0f;

    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider col;

    [SerializeField] private float rayLength = 0.5f;

    [SerializeField] private LayerMask layers;

    private float inputX;
    private float inputY;

    private Vector3 direction;
    private Vector3 originalCenter;
    private float originalHeight;

    private bool isGrounded;
    private bool isCrouched;
    private bool isSliding;

    [SerializeField] private float slidingSpeed = 5f;
    [SerializeField] private float slidingTime=5f;
    private float slidingTimer = 0f;
    private float slidingTimerLimit = 0f;

    [SerializeField] private float climbRayLength = 0.2f;
    [SerializeField] private LayerMask climbLayers;

    private bool isClimbing;

    private bool isClimbingRight;

    private AnimatorStateInfo stateInfo;
    private float climbingAnimationLength;

    private float climbingAnimationTimer;

//    private Vector3 climbPosition;

    //private bool isJumped;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb= GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        originalHeight = col.height;
        originalCenter = col.center;

        speed = OriginalSpeed;
    }

    private void Update()
    {
        
        GetInput();
        if (isGrounded || isClimbing)
        {
            direction = new Vector3(inputX, 0, inputY) * speed * Time.deltaTime;
            if (isClimbing)
            {
                if (direction.magnitude > 0f)
                {
                    direction = new Vector3(0,inputY, 0) * speed*Time.deltaTime;
                    //direction = new Vector3(0, 0, 0);
                }
            }
            if (direction.magnitude <= 0)
            {
                animator.SetFloat("speed", 0f);
            }
            else
            {
                if (isGrounded)
                {
                    animator.SetFloat("speed", 0.5f);
                    if (Input.GetKey(KeyCode.LeftShift) && !isClimbing)
                    {
                        animator.SetFloat("speed", 1f);
                        direction *= speed;
                    }
                }
            }

            transform.position += direction;
            if(isClimbing)
            {
                direction = new Vector3(inputX, 0f, inputY) * speed * Time.deltaTime;
            }
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.C) && !isClimbing)
            {
                if (!isCrouched)
                {
                    col.height = originalHeight / 2;
                    col.center = new Vector3(col.center.x,col.center.y/2,col.center.z);
                    isCrouched = true;
                    animator.SetBool("isCrouching", true);
                    if (direction.magnitude > 0)
                    {
                        //rb.AddForce(transform.forward * slidingSpeed, ForceMode.Impulse);
                        slidingTimerLimit = slidingTime;
                        isSliding = true;
                        animator.SetBool("isSliding", true);
                        animator.SetBool("isCrouching", false);
                    }
                }
                else
                {
                    isCrouched = false;
                    animator.SetBool("isCrouching", false);
                }
            }

            if (slidingTimer <= slidingTimerLimit && slidingTimerLimit>0f)
            {
                speed = slidingSpeed;
                slidingTimer += Time.deltaTime;
            }

            if ((!isCrouched || isSliding) && (direction.magnitude<=0f || slidingTimer>slidingTimerLimit))
            {
                slidingTimerLimit = 0f;
                slidingTimer = 0f;
                col.height = originalHeight;
                col.center = originalCenter;
                isCrouched = false;
                isSliding = false;
                speed = OriginalSpeed;
                animator.SetBool("isCrouching", false);
                animator.SetBool("isSliding", false);
            }
        }
    }

    private void FixedUpdate()
    {
        bool rayHit = Physics.Raycast(transform.position, Vector3.down, rayLength, layers);
        if (rayHit)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded=false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && rayHit)
        {
            animator.SetBool("isJumping",true);
            animator.SetFloat("jump", 0f);
            rb.AddForce(transform.up * 500f);
        }
        else if (rayHit)
        {
            animator.SetFloat("jump", 1f);
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetFloat("jump", 0.5f);
        }

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.forward, out hit, climbRayLength);
        if (hit.collider.gameObject.CompareTag("wall"))
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            climbingAnimationLength = stateInfo.length;
            Debug.Log(Time.deltaTime);
            if (climbingAnimationTimer >= climbingAnimationLength)
            {
                climbingAnimationTimer += Time.deltaTime;
            }
            //Debug.Log("hi");
            if (Input.GetKeyDown(KeyCode.C) && !isCrouched)
            {
                isClimbingRight = true;
                isClimbing = true;
                rb.useGravity = false;
                animator.SetBool("isClimbing", true);
            }

            if(isClimbing && isClimbingRight)
            {
                animator.SetBool("isClimbingRight", true);
                animator.SetBool("isClimbingLeft", false);
            }
            else if (isClimbing && !isClimbingRight)
            {
                //Debug.Log("hi");
                animator.SetBool("isClimbingRight", false);
                animator.SetBool("isClimbingLeft", true);
            }

            //Debug.Log(climbingAnimationTimer);

            if (climbingAnimationTimer > climbingAnimationLength)
            {
                climbingAnimationTimer = 0f;
                isClimbingRight = !isClimbingRight;
            }
        }
        else if(isClimbing)
        {
            transform.position += transform.forward*3f;
            isClimbing = false;
            rb.useGravity = true;
            animator.SetBool("isClimbing", false);
        }
    }

    private void GetInput()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical"); 
    }
}

