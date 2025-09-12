using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMapController : MonoBehaviour
{
    public Movement movement;
    public RiverMapsSO riverMapsSO;
    public List<RiverMap> riverMaps;
    public float defaultMapDistance;
    public Vector3 startingMapPosition;
    public int playerHardMapCurrentIndex=0;
    public Vector3 previousMapEndPosition;
    public int respawnBefore = 2;
    public List<Vector3> mapPositionsToRespawn;

    private float currentMapDistance;

    public void Awake()
    {
        startingMapPosition = transform.position;
        previousMapEndPosition = startingMapPosition;
        defaultMapDistance = 2000f;
        LoadRiverMapsFromSO();
        SortAndLoadRiverMaps();
        //ArrangeRiverMaps();
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
            previousMapEndPosition = mapsInfoController.endPosition.position;
            mapsInfoController.endPosition.transform.tag = "MapEnd";
        }

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

            riverMaps[i].SceneInstance.transform.position = mapPositionsToRespawn[i];
            riverMaps[randInt].SceneInstance.transform.position = mapPositionsToRespawn[randInt];
        }
        movement.RespawnPlayerForLoopFeel();
    }
}
