using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public HealthManager healthManager;
    public bool isInRiver;
    public float rayLength = 2f;
    public LayerMask layers;
    public void Awake()
    {
        currMovementSpeed = startMovementSpeed;
        actualRotation = transform.rotation;
        currRotationSpeed = startRotationSpeed;
    }

    public void Update()
    {
        CheckForRiver();
        if (isInRiver)
        {
            CheckForMovement();
            CheckForRotation();
        }
        else
        {
            healthManager.DecreaseHealth();
        }
    }

    public void CheckForRiver()
    {
        isInRiver = Physics.Raycast(transform.position, Vector3.down, rayLength, layers);
    }

    [Header("Movement")]

    //Movement


    public float maxMovementSpeed = 5f;
    public float startMovementSpeed = 0.1f;
    public float movementSpeedInc = 0.1f;

    private float currMovementSpeed;

    public void CheckForMovement()
    {
        if(canRotateLeft || canRotateRight)
        {
            currMovementSpeed += movementSpeedInc;
        }
        else
        {
            if (currMovementSpeed < startMovementSpeed)
            {
                currMovementSpeed = startMovementSpeed;
            }
            else
            {
                currMovementSpeed -= movementSpeedInc;
            }
        }

        if (currMovementSpeed >= maxMovementSpeed)
        {
            currMovementSpeed = maxMovementSpeed;
        }
        Move();
        
    }

    public void Move()
    {
        transform.position += transform.forward * currMovementSpeed;
    }

    [Header("Rotation")]

    //Rotation

    Quaternion actualRotation;
    public float maxRotationSpeed;
    public float rotationInc = 0.1f;
    public float startRotationSpeed = 0.1f;
    public float rotationSensivity;

    private float currRotationSpeed;
    public bool canRotateLeft;
    public bool canRotateRight;
   
    public void CheckForRotation()
    {
        canRotateLeft = RotateLeft();
        canRotateRight = RotateRight();

        if (!canRotateLeft && !canRotateRight)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, actualRotation, rotationSensivity * Time.deltaTime);
            currRotationSpeed -=rotationInc;
            if (currRotationSpeed < 0f)
            {
                currRotationSpeed = 0f;
            }
        }
    }

    public bool RotateLeft()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(-Vector3.up * currRotationSpeed);
            currRotationSpeed += rotationInc;
            //Debug.Log(currRotationSpeed);
            if (currRotationSpeed > maxRotationSpeed)
            {
                currRotationSpeed = maxRotationSpeed;
            }
            return true;
        }
        return false;
    }

    public bool RotateRight()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * currRotationSpeed);
            currRotationSpeed += rotationInc;
            //Debug.Log(currRotationSpeed);
            if (currRotationSpeed > maxRotationSpeed)
            {
                currRotationSpeed = maxRotationSpeed;
            }
            return true;
        }
        return false;
    }
}
