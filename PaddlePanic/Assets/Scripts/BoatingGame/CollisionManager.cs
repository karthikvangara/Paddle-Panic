using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Movement movement;
    public HealthManager healthManager;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            //healthManager.UpdateDamage(movement.currMovementSpeed);
        }
    }

}
