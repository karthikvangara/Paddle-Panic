using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public GameUIManager gameUIManager;
    public static CollectablesManager instance;

    public int collectablesCount;
    public void Awake()
    {
        instance = this;
    }

    public void UpdateCollectablesCount()
    {
        collectablesCount++;
        Debug.Log("Collectables Earned : " + collectablesCount);
        gameUIManager.UpdateCollectables();
    }
}
