using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Movement movement;
    public HealthManager healthManager;
    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided");
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            //healthManager.UpdateDamage(movement.currMovementSpeed);
            //float damage = 90 * (movement.overallPaddleForce / (movement.minPaddleForce * 2)) * (collision.gameObject.transform.localScale.magnitude / 100);
            //Debug.Log(damage);
            healthManager.CalculateHealth(movement.velocityMagnitude);
        }
    }


}
