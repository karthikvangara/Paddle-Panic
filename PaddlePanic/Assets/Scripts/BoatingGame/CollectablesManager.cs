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
        UserData userData = new UserData();
        userData = UserDataManager.instance.LoadPlayerInfo();
        userData.collectablesCount++;
        UserDataManager.instance.UpdatePlayerInfo(userData);
        gameUIManager.UpdateCollectables(userData.collectablesCount);
    }
}
