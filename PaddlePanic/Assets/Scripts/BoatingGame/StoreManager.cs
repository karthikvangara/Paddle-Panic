using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;
    public StoreData storeData;
    public ScrollRect scrollRect;
    private string path;

    public void Awake()
    {
        instance = this;
        path = Application.persistentDataPath + "/StoreData.json";
    }

    public void Start()
    {
        storeData=LoadStoreData();
        UpdateStoreData();
        LoadStoreItemsInGame();
    }

    public void LoadStoreItemsInGame()
    {
        storeData = LoadStoreData();
        Transform content=scrollRect.content;

        for (int i = 0; i < storeData.storeItemInfos.Count; i++)
        {
            GameObject storeItemObj=content.GetChild(i).gameObject;
            StoreItem storeItem=storeItemObj.GetComponent<StoreItem>();
            storeItem.itemId = storeData.storeItemInfos[i].itemId;
            storeItem.itemName = storeData.storeItemInfos[i].itemName;
            storeItem.itemCost = storeData.storeItemInfos[i].itemCost;
            storeItem.isPurchased = storeData.storeItemInfos[i].isPurchased;
            storeItem.isSelected = storeData.storeItemInfos[i].isSelected;
            storeItem.UpdateItemInfo();
        }
    }

    public void UpdateStoreData()
    {
        Debug.Log("UpdatePlayerInfo called");
        string json;
        if (!File.Exists(path))
        {
            json = JsonUtility.ToJson(storeData, true);
            File.WriteAllText(path, json);
            //Debug.Log("Player data File created");
        }

        if (storeData == null)
        {
            //Debug.Log("UserData is Null");
            return;
        }


        json = JsonUtility.ToJson(storeData, true);
        File.WriteAllText(path, json);
        //Debug.Log("Player data file Updatad");
    }

    public StoreData LoadStoreData()
    {
        Debug.Log("LoadPlayerInfo Called");
        if (storeData == null) Debug.Log("StoreData is Null");
        if (File.Exists(path))
        {
            string loadedJson = File.ReadAllText(path);
            storeData = JsonUtility.FromJson<StoreData>(loadedJson);
            //Debug.Log("Player Data Loaded");
        }
        return storeData;
    }

    public void OnClickBack()
    {
        UpdateStoreData();
        SceneManager.LoadSceneAsync("MainMenu");
    }

}

[Serializable]
public class StoreData
{
      public List<StoreItemInfo> storeItemInfos = new List<StoreItemInfo>();
}

[Serializable]

public class StoreItemInfo
{
    public int itemId;
    public string itemName;
    public bool isPurchased;
    public int itemCost;
    public bool isSelected;
}
