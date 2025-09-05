using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform boat;
    public float yDistance=5f;
    public float zDistance = 5f;

    public void Update()
    {
        transform.position = new Vector3(boat.position.x, boat.position.y - yDistance, boat.position.z - zDistance);
    }
}
