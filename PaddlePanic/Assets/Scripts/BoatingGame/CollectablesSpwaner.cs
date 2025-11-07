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

    public void Start()
    {
        xWidth=Mathf.RoundToInt(gridDimensions.x);
        yHeight=Mathf.RoundToInt(gridDimensions.y);
        GenerateGrid();
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(Mathf.RoundToInt(gridDimensions.x), 5, Mathf.RoundToInt(gridDimensions.y)));
        Gizmos.color = Color.cyan;
        for(int i = 0; i < validPositions.Count; i++)
        {
            Gizmos.DrawCube(validPositions[i],Vector3.one*nodeSize);
        }
    }
}
