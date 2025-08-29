using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMapController : MonoBehaviour
{
    public RiverMapsSO riverMapsSO;
    public List<GameObject> riverMaps;
    public float defaultMapDistance = 1000f;
    public Vector3 startingMapPosition;
    public int currentPlayersMapIndex;
    public int maxMapsLoadInGame;

    private float currentMapDistance;

    public void Awake()
    {
        currentPlayersMapIndex = -1;
        LoadRiverMapsFromSO();
        SortAndLoadRiverMaps();
        ArrangeRiverMaps();
    }

    public void LoadRiverMapsFromSO()
    {
        for (int i = 0; i < riverMapsSO.riverMapPrefabs.Count; i++)
        {
            riverMaps.Add(riverMapsSO.riverMapPrefabs[i]);
        }
    }

    public void SortAndLoadRiverMaps()
    {
        //Debug.Log("Karthik SortAndLoadRiverMaps");
        Vector3 instantiationPosition;
        for (int i = 0; i < riverMaps.Count-1; i++)
        {
            int randInt = Random.Range(i+1, riverMaps.Count);

            GameObject temp = riverMaps[i];
            riverMaps[i] = riverMaps[randInt];
            riverMaps[randInt] = temp;

            instantiationPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - defaultMapDistance);
            riverMaps[i] = Instantiate(riverMaps[i],instantiationPosition,Quaternion.identity);
        }

        instantiationPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - defaultMapDistance);
        riverMaps[^1] = Instantiate(riverMaps[^1], instantiationPosition, Quaternion.identity);

    }

    public void ArrangeRiverMaps()
    {
        //Debug.Log("Karthik ArrangeRiverMaps");
        for (int i = 0; i < riverMaps.Count; i++)
        {
            riverMaps[i].transform.position = new Vector3(startingMapPosition.x, startingMapPosition.y, startingMapPosition.z + currentMapDistance);
            currentMapDistance += defaultMapDistance;
        }
    }

    public void RespawnRiverMaps()
    {
        //Debug.Log("Karthik RespawnRiverMaps "+currentPlayersMapIndex);
        if (currentPlayersMapIndex > 0)
        {
            riverMaps[(currentPlayersMapIndex-1)%riverMaps.Count].transform.position = new Vector3(startingMapPosition.x, startingMapPosition.y, startingMapPosition.z + currentMapDistance);
            currentMapDistance += defaultMapDistance;
        }
    }
}
