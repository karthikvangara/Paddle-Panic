using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public HealthManager healthManager;
    public RiverMapController riverMapController;
    //public Vector3 recentCheckpointPosition;

    public void Awake()
    {
        //recentCheckpointPosition = transform.position;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MapEnd"))
        {
            //riverMapController.RespawnRiverMaps();
            riverMapController.playerHardMapCurrentIndex += 1;
            if (riverMapController.playerHardMapCurrentIndex % riverMapController.mapPositionsToRespawn.Count == 0) riverMapController.RespawnHardRiverMaps();
            //recentCheckpointPosition = new Vector3(other.gameObject.transform.position.x, transform.position.y, other.gameObject.transform.position.z);
        }
    }
}
