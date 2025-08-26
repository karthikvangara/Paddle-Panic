using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Transform startObject;
    public Transform targetObject;
    public GridHandler gridHandler;

    public void Update()
    {
        FindPath(startObject, targetObject);
    }
    public void FindPath(Transform startPos, Transform desPos)
    {
        Node startNode = gridHandler.GetPositionOnGrid(startPos);
        Node targetNode = gridHandler.GetPositionOnGrid(desPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count>0)
        {
            Node current=openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost || openSet[i].fCost==current.fCost && openSet[i].hCost<current.hCost)
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == targetNode)
            {
                GetPath(startNode,targetNode);
                return;
            }

            foreach(Node neighbour in gridHandler.GetNeighbours(current))
            {
                if(!neighbour.traversable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int distanceFromCurrentToNeighbour = GetDistanceFromTwoNodes(current, neighbour);

                if(distanceFromCurrentToNeighbour<neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = distanceFromCurrentToNeighbour;
                    neighbour.hCost = GetDistanceFromTwoNodes(current, targetNode);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    public void GetPath(Node startNode,Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node current = targetNode;

        while (current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        gridHandler.path = path;
        //Debug.Log(path.Count);
    }

    public int GetDistanceFromTwoNodes(Node startNode, Node targetNode)
    {
        int disX = Mathf.Abs(startNode.row - targetNode.row);
        int disY = Mathf.Abs(startNode.column - targetNode.column);

        if (disX <= disY)
        {
            return disX * 14 + disY * 10;
        }
        return disX * 10 + disY * 14;
    }
}
