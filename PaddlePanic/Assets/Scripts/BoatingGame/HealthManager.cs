using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
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

    /* public HealthUIManager healthUIManager;
     public float currentDamage;
     public float updateValue=15f;

     public void Update()
     {
         //Debug.Log(currentDamage);
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
     }*/

    public Movement movement;
    public GameUIManager gameUIManager;
    public float currHealth;
    public float maxHealth = 100f;
    public float maxBoatSpeed;
    public int impactCounts = 1;
    public float impactReduceBy = 0.1f;
    public bool isAlive = true;

    public void Start()
    {
        maxBoatSpeed = movement.maxVelocity;
        maxHealth = movement.maxVelocity;
        currHealth = maxHealth;

    }

    public void CalculateHealth(float boatSpeed)
    {
        currHealth -= boatSpeed * impactReduceBy;
        impactCounts += 1;
        gameUIManager.UpdateHealth();
        //Debug.Log(currHealth);
        if (currHealth <= 0.9f)
        {
            gameUIManager.OpenGameOverPanel();
            isAlive = false;
        }
        EnableInvincible();
    }

    [Header("Invinclibe Effect")]
    public List<Collider> invincibleColliders= new List<Collider>();
    public List<Renderer> invincibleRenders = new List<Renderer>();
    public int boatLayer, obstacleLayer;
    public float invincibleTime = 30f;
    public float invincibleTimer = 0f;
    public float alphaTime = 0.05f;

    public void EnableInvincible()
    {
        Physics.IgnoreLayerCollision(boatLayer, obstacleLayer, true);
        movement.rb.velocity = Vector3.zero;
        StartCoroutine(StartInvincible());

        if (invincibleTimer > invincibleTime)
        {
            invincibleTimer = 0;
            for (int i = 0; i < invincibleRenders.Count; i++)
            {
                invincibleRenders[i].enabled = true;
            }
        }
    }

    public IEnumerator StartInvincible()
    {
        for (invincibleTimer = 0; invincibleTimer < invincibleTime; invincibleTimer++)
        {
            for (int i = 0; i < invincibleRenders.Count; i++)
            {
                invincibleRenders[i].enabled = !invincibleRenders[i].enabled;
            }
            yield return new WaitForSeconds(alphaTime);
        }
        Physics.IgnoreLayerCollision(boatLayer, obstacleLayer, false);
    }
}
