using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    public LayerMask layers;
    public Vector2 gridDimensions;
    public float nodeSize = 1f;
    public Transform playerPosition;


    Vector3 pos;

    int possibleNumberOfNodes;
    public int xWidth;
    public int yHeight;

    Vector2 xRange;
    Vector2 yRange;

    public Node [,] grid;
    public List<Node> path;
    
    

    public void Start()
    {
        xWidth = Mathf.RoundToInt(gridDimensions.x);
        yHeight = Mathf.RoundToInt(gridDimensions.y);
        possibleNumberOfNodes = xWidth * yHeight;
        grid = new Node[xWidth, yHeight];
        CreateGrid();
        //Debug.Log(grid);
    }

    public void Update()
    {
        //GetPositionOnGrid(playerPosition);
    }

    public void CreateGrid()
    {
        pos = Vector3.left * (xWidth/2-nodeSize/2) - Vector3.forward * (yHeight/2-nodeSize/2);

        for (int i = 0; i < xWidth; i++)
        {
            for (int j = 0; j < yHeight; j++)
            {
                Vector3 tempPos = pos - Vector3.left * (i * nodeSize ) + Vector3.forward * (j * nodeSize);
                Node temp = new Node(i,j,true, tempPos,0,0);
                if (Physics.CheckSphere(tempPos, nodeSize/2, layers))
                {
                    temp.traversable = false;
                }
                grid[i,j] = temp;
            }
        }

    }

    public Node GetPositionOnGrid(Transform gameObject)
    {
        /*float playerXLeftEdgePos = gameObject.position.x - gameObject.localScale.x / 2;
        float playerXRightEdgePos = gameObject.position.x + gameObject.localScale.x / 2;

        float playerZBackEdgePos = gameObject.position.z - gameObject.localScale.z/2;
        float playerZFrontEdgePos = gameObject.position.z + gameObject.localScale.z / 2;

        xRange.x = Mathf.Abs(playerXLeftEdgePos - (-gridDimensions.x / 2))/nodeSize;
        xRange.y= Mathf.Abs(playerXRightEdgePos- (-gridDimensions.x / 2)) /nodeSize;

        yRange.x = Mathf.Abs(playerZBackEdgePos - (-gridDimensions.y / 2)) / nodeSize;
        yRange.y = Mathf.Abs(playerZFrontEdgePos-(-gridDimensions.y / 2)) / nodeSize;

        Debug.Log(xRange + " " + yRange);
        */
        
        float percentX = (gameObject.position.x + xWidth/2) / xWidth;
        float percentY = (gameObject.position.z + yHeight / 2) / yHeight;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((xWidth - 1)*percentX);
        int y = Mathf.RoundToInt((yHeight - 1) * percentY);

        //Debug.Log(x + " " + y);
        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node currentNode)
    {
        List<Node> neighbours = new List<Node>();

        for(int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if ((i==0 && j==0)||currentNode.row + i < 0 || currentNode.row + i >= xWidth || currentNode.column+j<0 || currentNode.column+j>=yHeight)
                {
                    continue;
                }
                neighbours.Add(grid[currentNode.row + i, currentNode.column + j]);
            }
        }
        return neighbours;

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(xWidth, 5, yHeight));

        for(int i = 0; i < xWidth/nodeSize; i++)
        {
            for(int j = 0; j < yHeight/nodeSize; j++)
            {
                Node playerNode = GetPositionOnGrid(playerPosition);
                Gizmos.color = Color.cyan;
                if (!grid[i, j].traversable)
                {
                    Gizmos.color = Color.red;
                }
                /*if(xRange.x<=i && xRange.y>=i && yRange.x<=j && yRange.y>=j)
                {
                    Gizmos.color = Color.black;
                }*/

                if (path.Contains(grid[i,j]))
                {
                    Gizmos.color = Color.black;
                }
                
                Gizmos.DrawCube(grid[i, j].worldPosition, Vector3.one*nodeSize);
            }
        }
    }
}

[System.Serializable]

public class Node
{
    public int row;
    public int column;
    public bool traversable;
    public Vector3 worldPosition;
    public float gCost;
    public float hCost;
    public Node parent;

    public Node(int _row,int _column ,bool canTraverse, Vector3 worldPos,float _gCost, float _hCost)
    {
        row = _row;
        column = _column;
        traversable = canTraverse;
        worldPosition = worldPos;
        gCost = _gCost;
        hCost = _hCost;
    }

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
