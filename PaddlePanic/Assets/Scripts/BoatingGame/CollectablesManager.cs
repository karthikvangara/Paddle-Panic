using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public GameUIManager gameUIManager;
    public static CollectablesManager instance;
    public void Awake()
    {
        instance = this;
    }

    public void UpdateCollectablesCount()
    {
        UserDataManager.instance.LoadPlayerInfo();
        UserDataManager.instance.userData.collectablesCount++;
        UserDataManager.instance.UpdatePlayerInfo();
        gameUIManager.UpdateCollectables(UserDataManager.instance.userData.collectablesCount);
    }
}
