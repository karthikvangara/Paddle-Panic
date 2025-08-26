using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Movement movement;
    public CheckPointManager checkPointManager;
    public int maxHealth = 3;
    public int currHealth;

    public void Awake()
    {
        currHealth = maxHealth;
    }

    public void IncreaseHealth()
    {
        if (currHealth < maxHealth)
        {
            currHealth += 1;
        }
    }

    public void DecreaseHealth()
    {
        currHealth -= 1;
        Respawn();
        if (currHealth <= 0)
        {
            string getCurrentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(getCurrentScene);
        }
    }

    public void Respawn()
    {
        if (checkPointManager.recentCheckpointPosition != null || !movement.isInRiver)
        {
            transform.position = checkPointManager.recentCheckpointPosition;
        }
    }
}
