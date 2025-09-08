using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMovement : MonoBehaviour
{
    public LayerMask layers;
    public float rayLength = 20f;



    public void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, rayLength, layers))
        {
            
        }
    }
}
