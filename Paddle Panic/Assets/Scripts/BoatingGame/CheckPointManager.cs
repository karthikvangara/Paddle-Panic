using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public Vector3 recentCheckpointPosition;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            recentCheckpointPosition = new Vector3(other.gameObject.transform.position.x, transform.position.y, other.gameObject.transform.position.z);
        }
    }
}
