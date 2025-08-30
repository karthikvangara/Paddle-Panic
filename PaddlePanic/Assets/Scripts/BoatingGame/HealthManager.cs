using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    /*public Movement movement;
    public CheckPointManager checkPointManager;
    public int maxHealth = 5;
    public int currHealth;
    public bool isRespawning;

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
        ReloadScene();
    }

    public void Respawn()
    {
        Debug.Log("Respawn");
        isRespawning = true;
        if (checkPointManager.recentCheckpointPosition != null || !movement.isInRiver)
        {
            transform.position = checkPointManager.recentCheckpointPosition;
        }
    }

    public void ReloadScene()
    {
        if (currHealth <= 0)
        {
            string getCurrentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(getCurrentScene);
        }
    }*/

    public HealthUIManager healthUIManager;
    public float currentDamage;
    public float updateValue=15f;

    public void Update()
    {
        Debug.Log(currentDamage);
        if (currentDamage >= 100)
        {
            healthUIManager.Display0PerHealth();
        }
        else if(currentDamage >= 80)
        {
            healthUIManager.Display20PerHealth();
        }
        else if(currentDamage >= 60)
        {
            healthUIManager.Display40PerHealth();
        }
        else if(currentDamage >= 40)
        {
            healthUIManager.Display60PerHealth();
        }
        else if(currentDamage >= 20)
        {
            healthUIManager.Display80PerHealth();
        }
        else
        {
            healthUIManager.Display100PerHealth();
        }
    }

    public void UpdateDamage(float boatSpeed)
    {
        currentDamage = currentDamage+boatSpeed * updateValue*Time.deltaTime;
    }
}
