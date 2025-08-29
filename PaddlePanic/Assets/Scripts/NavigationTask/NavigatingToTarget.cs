using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatingToTarget : MonoBehaviour
{
    public GridHandler gridHandler;
    public float speed = 1f;

    public void Update()
    {
        if (gridHandler.path != null && gridHandler.path.Count>=1)
        {
            Move();
        }
    }

    public void Move()
    {
        transform.LookAt(gridHandler.path[0].worldPosition);
        transform.position += transform.forward * speed;
    }
}
