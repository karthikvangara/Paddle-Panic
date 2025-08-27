using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public HealthManager healthManager;
    public RiverMapController riverMapController;
    public Vector3 recentCheckpointPosition;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            //Debug.Log("Karthik Checkpoint exited ");
            if (!healthManager.isRespawning)
            {
                riverMapController.currentPlayersMapIndex += 1;
                riverMapController.RespawnRiverMaps();
            }
            recentCheckpointPosition = new Vector3(other.gameObject.transform.position.x, transform.position.y, other.gameObject.transform.position.z);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            healthManager.isRespawning = false;
        }
    }
}
