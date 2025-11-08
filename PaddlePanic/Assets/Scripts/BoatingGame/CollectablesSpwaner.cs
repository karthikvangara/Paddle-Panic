using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesSpwaner : MonoBehaviour
{
    public Vector2 gridDimensions;
    public int xWidth, yHeight;
    public float gridStart=215;
    public float nodeSize;
    public LayerMask obstacleLayers;
    public List<Vector3> validPositions = new List<Vector3>();
    public List<GameObject> collectables=new List<GameObject>();
    public float spawnPercentage = 100f;
    public int spawingCount;
    private Dictionary<int,Vector3> selectedPositions=new Dictionary<int,Vector3>();
    public List<Vector3> spawingPositions = new List<Vector3>();
    public List<GameObject> instantiatedCollectables = new List<GameObject>();

    public void Start()
    {
        xWidth=Mathf.RoundToInt(gridDimensions.x);
        yHeight=Mathf.RoundToInt(gridDimensions.y);
        GenerateGrid();
        SelectSpawingPositions();
        SpawnCollectables();
    }

    public void GenerateGrid()
    {
        Vector3 pos= Vector3.left * (xWidth / 2 - nodeSize / 2) + Vector3.forward*(transform.position.z-gridStart);
        validPositions.Add(pos);
        for (int i = 0; i < xWidth/nodeSize; i++)
        {
            for (int j = 0; j < yHeight/nodeSize; j++)
            {
                Vector3 tempPos = pos - (Vector3.left * (i * nodeSize))-(Vector3.forward*(j*nodeSize));

                if (!Physics.CheckSphere(tempPos, nodeSize, obstacleLayers))
                {
                    validPositions.Add(tempPos);
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        /*Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(Mathf.RoundToInt(gridDimensions.x), 5, Mathf.RoundToInt(gridDimensions.y)));
        Gizmos.color = Color.cyan;
        for(int i = 0; i < validPositions.Count; i++)
        {
            Gizmos.DrawCube(validPositions[i],Vector3.one*nodeSize);
        }

        Gizmos.color = Color.green;

        for(int i = 0; i < spawingPositions.Count; i++)
        {
            Gizmos.DrawCube(spawingPositions[i],Vector3.one*nodeSize);
        }*/
    }

    public void SelectSpawingPositions()
    {
        spawingCount=Mathf.RoundToInt((spawnPercentage/100)*validPositions.Count);
        //Debug.Log(spawingCount);
        PickRandomPositions();

        foreach(var value in selectedPositions.Values)
        {
            spawingPositions.Add(value);
        }
    }

    public void PickRandomPositions()
    {
        while (selectedPositions.Count < spawingCount)
        {
            int randIdx = UnityEngine.Random.Range(0, validPositions.Count);
            if (!selectedPositions.ContainsKey(randIdx))
            {
                selectedPositions.Add(randIdx, validPositions[randIdx]);
            }
        }
    }

    public void SpawnCollectables()
    {
        for (int i = 0; i < spawingPositions.Count; i++)
        {
            Vector3 pos= spawingPositions[i];
            GameObject collectable=collectables[UnityEngine.Random.Range(0,collectables.Count)];

            GameObject instantiatedCollectable=Instantiate(collectable,pos,Quaternion.identity);
            instantiatedCollectables.Add(instantiatedCollectable);
            Collectable instantiatedCollectableScript=instantiatedCollectable.GetComponent<Collectable>();
            instantiatedCollectableScript.jumpPositions.Add(pos-Vector3.left*(nodeSize/2));
            instantiatedCollectableScript.jumpPositions.Add(pos-Vector3.forward*(nodeSize/2));
            instantiatedCollectableScript.jumpPositions.Add(pos+Vector3.left*(nodeSize/2));
            instantiatedCollectableScript.jumpPositions.Add(pos+Vector3.forward*(nodeSize/2));
        }
    }
}
