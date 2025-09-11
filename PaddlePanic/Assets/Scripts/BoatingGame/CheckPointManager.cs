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
        if (other.gameObject.CompareTag("HardMapEndPosition"))
        {
            //riverMapController.RespawnRiverMaps();
            riverMapController.playerHardMapCurrentIndex += 1;
            riverMapController.RespawnHardRiverMaps();
            //recentCheckpointPosition = new Vector3(other.gameObject.transform.position.x, transform.position.y, other.gameObject.transform.position.z);
        }
    }
}
