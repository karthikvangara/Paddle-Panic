using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMapController : MonoBehaviour
{
    public RiverMapsSO riverMapsSO;
    public List<GameObject> easyRiverMaps;
    public List<GameObject> mediumRiverMaps;
    public List<GameObject> hardRiverMaps;
    public float defaultMapDistance;
    public Vector3 startingMapPosition;
    public int playerHardMapCurrentIndex;
    public Vector3 previousMapEndPosition; 

    private float currentMapDistance;

    public void Awake()
    {
        previousMapEndPosition = startingMapPosition;
        defaultMapDistance = 2000f;
        playerHardMapCurrentIndex = -1;
        LoadRiverMapsFromSO();
        SortAndLoadRiverMaps();
        //ArrangeRiverMaps();
    }

    public void LoadRiverMapsFromSO()
    {
        for (int i = 0; i < riverMapsSO.easyRiverMapPrefabs.Count; i++)
        {
            easyRiverMaps.Add(riverMapsSO.easyRiverMapPrefabs[i]);
        }
        for (int i = 0; i < riverMapsSO.mediumRiverMapPrefabs.Count; i++)
        {
            mediumRiverMaps.Add(riverMapsSO.mediumRiverMapPrefabs[i]);
        }
        for (int i = 0; i < riverMapsSO.hardRiverMapPrefabs.Count; i++)
        {
            hardRiverMaps.Add(riverMapsSO.hardRiverMapPrefabs[i]);
        }
    }

    public void SortAndLoadRiverMaps()
    {
        //Debug.Log("Karthik SortAndLoadRiverMaps");
        Vector3 instantiationPosition;

        //Easy
        for (int i = 0; i < easyRiverMaps.Count; i++)
        {
            int randInt = i;
            if (i < easyRiverMaps.Count - 1)
            {
                randInt = Random.Range(i + 1, easyRiverMaps.Count);
            }
            GameObject temp = easyRiverMaps[i];
            easyRiverMaps[i] = easyRiverMaps[randInt];
            easyRiverMaps[randInt] = temp;

            instantiationPosition = previousMapEndPosition;
            easyRiverMaps[i] = Instantiate(easyRiverMaps[i],instantiationPosition,Quaternion.identity);
            MapsInfoController mapsInfoController = easyRiverMaps[i].GetComponent<MapsInfoController>();
            previousMapEndPosition = mapsInfoController.endPosition.position;
        }

        //Medium
        for (int i = 0; i < mediumRiverMaps.Count; i++)
        {
            int randInt = i;
            if (i < mediumRiverMaps.Count - 1)
            {
                randInt = Random.Range(i + 1, mediumRiverMaps.Count);
            }
            GameObject temp = mediumRiverMaps[i];
            mediumRiverMaps[i] = mediumRiverMaps[randInt];
            mediumRiverMaps[randInt] = temp;

            instantiationPosition = previousMapEndPosition;
            mediumRiverMaps[i] = Instantiate(mediumRiverMaps[i], instantiationPosition, Quaternion.identity);
            MapsInfoController mapsInfoController = mediumRiverMaps[i].GetComponent<MapsInfoController>();
            previousMapEndPosition = mapsInfoController.endPosition.position;
        }

        //Hard
        for (int i = 0; i < hardRiverMaps.Count; i++)
        {
            int randInt = i;
            if (i < hardRiverMaps.Count - 1)
            {
                randInt = Random.Range(i + 1, hardRiverMaps.Count);
            }
            GameObject temp = hardRiverMaps[i];
            hardRiverMaps[i] = hardRiverMaps[randInt];
            hardRiverMaps[randInt] = temp;

            instantiationPosition = previousMapEndPosition;
            hardRiverMaps[i] = Instantiate(hardRiverMaps[i], instantiationPosition, Quaternion.identity);
            MapsInfoController mapsInfoController = hardRiverMaps[i].GetComponent<MapsInfoController>();
            previousMapEndPosition = mapsInfoController.endPosition.position;
            mapsInfoController.endPosition.gameObject.tag = "HardMapEndPosition";
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
        //Debug.Log("Karthik RespawnRiverMaps "+currentPlayersMapIndex);
        if (playerHardMapCurrentIndex >= 0)
        {
            hardRiverMaps[(playerHardMapCurrentIndex)%hardRiverMaps.Count].transform.position = previousMapEndPosition;
            MapsInfoController mapsInfoController = hardRiverMaps[(playerHardMapCurrentIndex) % hardRiverMaps.Count].GetComponent<MapsInfoController>();
            previousMapEndPosition = mapsInfoController.endPosition.position;
        }
    }
}
