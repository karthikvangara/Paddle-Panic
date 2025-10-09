using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMapController : MonoBehaviour
{
    public Movement movement;
    public CameraRespawnHelper cameraRespawnHelper;
    public RiverMapsSO riverMapsSO;
    public List<RiverMap> riverMaps;
    public Vector3 startingMapPosition;
    public int playerCurrentMapIndex=0;
    public Vector3 previousMapEndPosition;
    public int respawnBefore = 2;
    public List<Vector3> mapPositionsToRespawn;
    public int enableNumberOfNextMaps = 2;
    public int disableNumberOfPreviousMaps = 2;
    public bool isRiverMapsLoaded;

    public void Awake()
    {
        startingMapPosition = transform.position;
        previousMapEndPosition = startingMapPosition;
        
        //StartCoroutine(StartLoadingRiverMaps());
        
        LoadRiverMapsFromSO();
        SortAndLoadRiverMaps();
        //ArrangeRiverMaps();
    }

    IEnumerator StartLoadingRiverMaps()
    {
        yield return new WaitForSeconds(0.1f);
        LoadRiverMapsFromSO();
        SortAndLoadRiverMaps();
    }

    public void LoadRiverMapsFromSO()
    {
        for (int i = 0; i < riverMapsSO.riverMapPrefabsFromEasyToHard.Count; i++)
        {
            riverMaps.Add(riverMapsSO.riverMapPrefabsFromEasyToHard[i]);
        }
    }

    public void SortAndLoadRiverMaps()
    {
        //Debug.Log("Karthik SortAndLoadRiverMaps");
        Vector3 instantiationPosition;

        for (int i = 0; i < riverMaps.Count; i++)
        {
            int randInt = i;
            if (riverMaps[i].mapDifficulty==RiverMapDifficulty.Hard && i<riverMaps.Count-1)
            {
                randInt = Random.Range(i + 1, riverMaps.Count);
            }
            RiverMap temp = riverMaps[i];
            riverMaps[i] = riverMaps[randInt];
            riverMaps[randInt] = temp;

            if (mapPositionsToRespawn.Count < riverMaps.Count - respawnBefore)
            {
                mapPositionsToRespawn.Add(previousMapEndPosition);
            }
            instantiationPosition = previousMapEndPosition;
            riverMaps[i].SceneInstance = Instantiate(riverMaps[i].Map, instantiationPosition,Quaternion.identity);
            MapsInfoController mapsInfoController = riverMaps[i].SceneInstance.GetComponent<MapsInfoController>();
            previousMapEndPosition = new Vector3(mapsInfoController.endPosition.position.x,0f,mapsInfoController.endPosition.position.z);
            mapsInfoController.endPosition.transform.tag = "MapEnd";

            if (i > enableNumberOfNextMaps && i<riverMaps.Count-respawnBefore)
            {
                riverMaps[i].SceneInstance.SetActive(false);
            }
        }
        isRiverMapsLoaded = true;
    }

    /*public void ArrangeRiverMaps()
    {
        //Debug.Log("Karthik ArrangeRiverMaps");
        for (int i = 0; i < riverMaps.Count; i++)
        {
            riverMaps[i].transform.position = new Vector3(startingMapPosition.x, startingMapPosition.y, startingMapPosition.z + currentMapDistance);
            currentMapDistance += defaultMapDistance;
        }
    }*/
    public void EnableNextMaps()
    {
        for(int i = 1; i <= enableNumberOfNextMaps; i++)
        {
            int idx = (playerCurrentMapIndex % mapPositionsToRespawn.Count) + i;
            if (idx < riverMaps.Count)
            {
                riverMaps[idx].SceneInstance.SetActive(true);
            }
        }
    }

    public void DisablePreviousMap()
    {
        int idx = (playerCurrentMapIndex - disableNumberOfPreviousMaps) % mapPositionsToRespawn.Count;
        if (idx >= 0) {
            riverMaps[idx].SceneInstance.SetActive(false);
        }
    }

    public void RespawnHardRiverMaps()
    {
        Debug.Log("Respawned");
        Vector3 tempPosition;

        for (int i = 0; i < mapPositionsToRespawn.Count; i++)
        {
            int randInt = i;
            if (i< mapPositionsToRespawn.Count-1)
            {
                randInt = Random.Range(i + 1, mapPositionsToRespawn.Count);
            }

            RiverMap tempMap = riverMaps[i];
            riverMaps[i] = riverMaps[randInt];
            riverMaps[randInt] = tempMap;

            if (i > enableNumberOfNextMaps)
            {
                riverMaps[i].SceneInstance.SetActive(false);
            }
            else
            {
                riverMaps[i].SceneInstance.SetActive(true);

            }
            riverMaps[i].SceneInstance.transform.position = mapPositionsToRespawn[i];
            riverMaps[randInt].SceneInstance.transform.position = mapPositionsToRespawn[randInt];
        }
        //cameraRespawnHelper.SaveCameraState();
        movement.RespawnPlayerForLoopFeel();
    }
}
