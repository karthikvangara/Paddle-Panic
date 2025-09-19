using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public RiverMapController riverMapController;
    public Rigidbody rb;
    public GameObject player1InputController;
    public GameObject player2InputController;
    public HealthManager healthManager;
    public bool isInRiver;
    public float rayLength = 2f;
    public LayerMask layers;
    public bool isLeftPressed;
    public bool isRightPressed;

    private TouchField player1TouchField;
    private TouchField player2TouchField;
    public void Awake()
    {
        playerStartingPosition = transform.position;
        player1TouchField = player1InputController.GetComponent<TouchField>();
        player2TouchField = player2InputController.GetComponent<TouchField>();
        //currMovementSpeed = startMovementSpeed;
        actualRotation = transform.rotation;
        //currRotationSpeed = startRotationSpeed;
    }

    public void Update()
    {
        /*CheckForInput();
        if (isInRiver)
        {
            CheckForMovement();
            CheckForRotation();
        }
        else
        {
            //healthManager.DecreaseHealth();
        }*/
    }

    public void FixedUpdate()
    {
        CheckForRiver();
        CheckForInput();
        if (isInRiver)
        {
            CheckForMovement();
            CheckForRotation();
        }
        else
        {
            //healthManager.DecreaseHealth();
        }
    }

    public void CheckForInput()
    {
        isLeftPressed = player1TouchField.Pressed;
        isRightPressed = player2TouchField.Pressed;

        Debug.Log(isLeftPressed);
        Debug.Log(isRightPressed);

        /*if (Input.GetKey(KeyCode.RightArrow)) isRightPressed = true;
        else isRightPressed = false;
      

        if (Input.GetKey(KeyCode.LeftArrow)) isLeftPressed = true;
        else isLeftPressed = false;*/
    }

    public void CheckForRiver()
    {
        isInRiver = Physics.Raycast(transform.position, Vector3.down, rayLength, layers);
        //Debug.Log(isInRiver);
    }


    [Header("Respawn Map")]

    public Vector3 playerStartingPosition;
    public Vector3 playerPositionBeforeRespawn;

    public void RespawnPlayerForLoopFeel()
    {
        playerPositionBeforeRespawn = transform.position;
        transform.position = new Vector3(playerPositionBeforeRespawn.x, playerPositionBeforeRespawn.y, playerStartingPosition.z);
    }

    [Header("Movement")]

    //Movement

    public float maxPaddleForce = 360f;
    public float minPaddleForce = 10f;
    public float incPaddleForce = 1f;
    public float leftPaddleForce = 0f;
    public float rightPaddleForce = 0f;
    public float overallPaddleForce = 0f;
    public float maxVelocity = 1500f;
    public float minVelocity = 15f;
    public float velocityMagnitude;
    //public float dragCoefficient = 5f;
    public float drag = 0f;
    //public float angularDrag = 2f;
    public float driftFactor = 0.9f;
    public float maxJump = 5f;

    /*public float maxMovementSpeed = 5f;
    public float startMovementSpeed = 0.1f;
    public float movementSpeedInc = 0.1f;
    public float movementOpposingForce = 10f;

    public float currMovementSpeed;*/

    public void CheckForMovement()
    {
        //OpposeMotion();
        /*if (isLeftPressed) currMovementSpeed += movementSpeedInc;
        if (isRightPressed) currMovementSpeed += movementSpeedInc;
        if (!isRightPressed && !isLeftPressed)  //OpposeMotion();
        //if (!isRightPressed && !isLeftPressed) OpposeMotion();
        if (currMovementSpeed < startMovementSpeed) currMovementSpeed = startMovementSpeed;
        if (currMovementSpeed > maxMovementSpeed) currMovementSpeed = maxMovementSpeed;*/

        leftPaddleForce = isLeftPressed ? leftPaddleForce + incPaddleForce : (leftPaddleForce - incPaddleForce) *0.9f >0? (leftPaddleForce-incPaddleForce) * 0.9f:0;
        rightPaddleForce = isRightPressed ? rightPaddleForce + incPaddleForce : (rightPaddleForce - incPaddleForce) *0.9f > 0 ? (rightPaddleForce - incPaddleForce) * 0.9f : 0;

        leftPaddleForce = Mathf.Min(leftPaddleForce, maxPaddleForce);
        rightPaddleForce = Mathf.Min(rightPaddleForce, maxPaddleForce);

        //leftPaddleForce = Mathf.Max(leftPaddleForce, minPaddleForce);
        //rightPaddleForce = Mathf.Max(rightPaddleForce, minPaddleForce);
        
        overallPaddleForce = leftPaddleForce + rightPaddleForce;
        overallPaddleForce = Mathf.Max(overallPaddleForce, 0);

        //drag = dragCoefficient * rb.velocity.magnitude;

        velocityMagnitude = rb.velocity.magnitude;
        if (isLeftPressed || isRightPressed) ApplyForwardForceToBoat();
        ControlVelocity();   //  Control Moving Forward
        //ControlVelocity();  // Control Max and Min velocity
        //ApplyAngularDrag();
        //if (!isLeftPressed && !isRightPressed && isInRiver) ApplyDragToBoat();   //  Controls veclocity when nothing is pressed
        ControlDrift();  //  Controls velocity when only one side is pressed
        ControlJumpHeight();    // Controls player jump height 


    }

    public void ApplyForwardForceToBoat()
    {
        //transform.position += transform.forward * currMovementSpeed;
        //rb.AddForce(transform.forward*currMovementSpeed*2, ForceMode.Force);
        rb.AddForce(rb.transform.forward * overallPaddleForce, ForceMode.Force);

        Debug.DrawRay(rb.transform.position, rb.transform.forward * 3000f,Color.white);

    }
    public void ControlVelocity()
    {
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.transform.forward * maxVelocity;
        }

        /*if (rb.velocity.magnitude < minVelocity)
        {
            rb.velocity = rb.transform.forward * minVelocity;
        }*/
        rb.AddForce(-rb.transform.forward * overallPaddleForce/minPaddleForce, ForceMode.Force);
    }

    public void ControlDrift()
    {
        Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
        localVel.x *= driftFactor;
        rb.velocity = transform.TransformDirection(localVel);
    }
    /*
    public void ApplyDragToBoat()
    {
        //Debug.Log("hi");
        //rb.AddForce(-rb.transform.forward * (overallPaddleForce/2), ForceMode.Acceleration);
        rb.velocity -= rb.transform.forward * drag;
        ControlVelocity();

    }*/  

    public void ControlJumpHeight()
    {
        if (transform.position.y > playerStartingPosition.y + maxJump)
        {
            Debug.Log("jumped");
            rb.velocity = new Vector3((rb.velocity.x/rb.velocity.magnitude)*maxJump, 0f, (rb.velocity.z/rb.velocity.magnitude)*maxJump);
            //rb.AddForce(-transform.up * overallPaddleForce, ForceMode.Force);
        }
    }




    /*public void ApplyAngularDrag()
    {
        rb.angularDrag = angularDrag;
    }
    */


    /*public void OpposeMotion()
    {
        currMovementSpeed -= movementSpeedInc;
        Debug.Log(rb.velocity.magnitude);
        rb.AddForce((-transform.forward) * currMovementSpeed, ForceMode.Force);
    }*/

    [Header("Rotation")]

    //Rotation

    public Quaternion actualRotation;
    /*public float currRotationAngle=0f;
    public float incRotationAngle=1f;*/
    //public float turnVelocity = 2f;
    //public float rotationAngle = 0f;
    //public float rotateLeftAngle = 0f;
    //public float rotateRightAngle = 0f;
    //public float rotationMultiplyer = 5f;
    public float rotationRetrivalMultiplyer = 10f;
    //public float diffPaddleForce = 0f;

    /*public float maxRotationSpeed;
    public float startRotationSpeed = 0.1f;
    public float rotationInc = 0.1f;

    private float currRotationSpeed;*/
   
    public void CheckForRotation()
    {
        /*if (isLeftPressed && isRightPressed) { }
        else
        {
            if (isRightPressed) RotateLeft();
            if (isLeftPressed) RotateRight();
        }
        if (!isLeftPressed && !isRightPressed) RetrieveRotation();*/

        //rotateLeftAngle = rightPaddleForce / 360;
        //rotateRightAngle = leftPaddleForce / 360;
        //rotationAngle = rotateRightAngle-rotateLeftAngle;
        //Debug.Log(rotationAngle);
        //if(isRightPressed) RotateLeft();
        //if(isLeftPressed) RotateRight();
        if (!isLeftPressed && !isRightPressed) RetrieveRotation();
        //if(isRightPressed) RotateLeft(); 
        //if(isLeftPressed) RotateRight();
        //diffPaddleForce = leftPaddleForce - rightPaddleForce;
        //Rotate();
        if (isRightPressed && !isLeftPressed) RotateLeft();
        if (isLeftPressed && !isRightPressed) RotateRight();
    }


    /*public void RotateLeft()
    {
        //transform.Rotate(-transform.up * overallPaddleForce*0.5f*Time.deltaTime);
        currRotationAngle -= incRotationAngle;
    }

    public void RotateRight()
    {
        //transform.Rotate(transform.up * overallPaddleForce*0.5f*Time.deltaTime);
        currRotationAngle += incRotationAngle;
    }*/

    /*public void Rotate()
    {
        rb.AddRelativeTorque(0f, diffPaddleForce*rotationMultiplyer,0f,ForceMode.Acceleration);

        float y = Mathf.Clamp(rb.angularVelocity.y, -2f, 2f);
        rb.angularVelocity = new Vector3(0f,y, 0f);

        Quaternion current = rb.rotation;
        Quaternion target = Quaternion.Euler(0, current.eulerAngles.y, 0);
        rb.rotation = Quaternion.Slerp(current, target, Time.fixedDeltaTime * 2f);
    }
    */
    public void RotateLeft()
    {
        //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currRotationAngle, ref turnVelocity, 0.1f);
        /*float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, (transform.eulerAngles.y-rotateLeftAngle*rotationMultiplyer), ref turnVelocity, 0.1f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);*/
        //rb.AddTorque(-Vector3.up *rightPaddleForce*rotationMultiplyer,ForceMode.Force);
        Vector3 direction = new Vector3(-rightPaddleForce, 0f, 0f);
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation,targetRotation,0.01f));
        }
    }

    public void RotateRight()
    {
        /*float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, (transform.eulerAngles.y + rotateRightAngle * rotationMultiplyer), ref turnVelocity, 0.1f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);*/
        //rb.AddTorque(Vector3.up *leftPaddleForce*rotationMultiplyer, ForceMode.Force);
        Vector3 direction = new Vector3(leftPaddleForce, 0f, 0f);
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 0.01f));
        }
    }

    /*public void RotateLeft()
    {
        //transform.Rotate(-Vector3.up * currRotationSpeed);
        currRotationSpeed += rotationInc;
        //Debug.Log(currRotationSpeed);
        if (currRotationSpeed > maxRotationSpeed)
        {
            currRotationSpeed = maxRotationSpeed;
        }

        rb.angularVelocity=new Vector3(0f,-currRotationSpeed,0f)*Time.deltaTime;
    }

    public void RotateRight()
    {
        //transform.Rotate(Vector3.up * currRotationSpeed);
        currRotationSpeed += rotationInc;
        //Debug.Log(currRotationSpeed);
        if (currRotationSpeed > maxRotationSpeed)
        {
            currRotationSpeed = maxRotationSpeed;
        }
        rb.angularVelocity = new Vector3(0f, currRotationSpeed, 0f)*Time.deltaTime;
    }
    */
    public void RetrieveRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, actualRotation, rotationRetrivalMultiplyer * Time.deltaTime);
    }
}
