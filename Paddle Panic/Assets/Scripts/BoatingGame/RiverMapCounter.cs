using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMapCounter : MonoBehaviour
{
    public RiverMapController riverMapController;

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            Debug.Log("Karthik " + other.transform.parent.parent.name);
            riverMapController.currentPlayersMapIndex += 1;
            riverMapController.RespawnRiverMaps();
        }
    }
}
