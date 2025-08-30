using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
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
        player1TouchField = player1InputController.GetComponent<TouchField>();
        player2TouchField = player2InputController.GetComponent<TouchField>();
        currMovementSpeed = startMovementSpeed;
        actualRotation = transform.rotation;
        currRotationSpeed = startRotationSpeed;
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

        if (Input.GetKey(KeyCode.RightArrow)) isRightPressed = true;
        else isRightPressed = false;
      

        if (Input.GetKey(KeyCode.LeftArrow)) isLeftPressed = true;
        else isLeftPressed = false;
    }

    public void CheckForRiver()
    {
        isInRiver = Physics.Raycast(transform.position, Vector3.down, rayLength, layers);
        Debug.Log(isInRiver);
    }

    [Header("Movement")]

    //Movement


    public float maxMovementSpeed = 5f;
    public float startMovementSpeed = 0.1f;
    public float movementSpeedInc = 0.1f;

    public float currMovementSpeed;

    public void CheckForMovement()
    {
        if (isLeftPressed) currMovementSpeed += movementSpeedInc;
        if (isRightPressed) currMovementSpeed += movementSpeedInc;
        //if (!isRightPressed && !isLeftPressed) currMovementSpeed -= movementSpeedInc/2;
        if (!isRightPressed && !isLeftPressed) currMovementSpeed = 0;
        if (currMovementSpeed < startMovementSpeed) currMovementSpeed = startMovementSpeed;
        if (currMovementSpeed > maxMovementSpeed) currMovementSpeed = maxMovementSpeed;
        Move();
        
    }

    public void Move()
    {
        //transform.position += transform.forward * currMovementSpeed;
        rb.AddForce(transform.forward*currMovementSpeed, ForceMode.Force);
    }

    [Header("Rotation")]

    //Rotation

    Quaternion actualRotation;
    public float maxRotationSpeed;
    public float rotationInc = 0.1f;
    public float startRotationSpeed = 0.1f;
    public float rotationSensivity;

    private float currRotationSpeed;
   
    public void CheckForRotation()
    {
        if (isLeftPressed && isRightPressed) { }
        else
        {
            if (isRightPressed) RotateLeft();
            if (isLeftPressed) RotateRight();
        }
        if (!isLeftPressed && !isRightPressed) RetrieveRotation();
        
    }

    public void RotateLeft()
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

    public void RetrieveRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, actualRotation, rotationSensivity * Time.deltaTime);
        currRotationSpeed -= rotationInc;
        if (currRotationSpeed < 0f)
        {
            currRotationSpeed = 0f;
        }
    }
}
