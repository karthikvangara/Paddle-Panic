using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public HealthManager healthManager;
    public RiverMapController riverMapController;
    public Vector3 recentCheckpointPosition;

    public void Awake()
    {
        recentCheckpointPosition = transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            riverMapController.RespawnRiverMaps();
            riverMapController.currentPlayersMapIndex += 1;
            recentCheckpointPosition = new Vector3(other.gameObject.transform.position.x, transform.position.y, other.gameObject.transform.position.z);
        }
    }
}
