using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;
    public UserData userData;
    private string path;

    public void Awake()
    {
        instance = this;
        path = Application.persistentDataPath + "/UserData.json";
    }
    public void UpdatePlayerInfo()
    {
        //Debug.Log("UpdatePlayerInfo called");
        string json;
        if (!File.Exists(path))
        {
            userData.playerName = "Hi Bro";
            json = JsonUtility.ToJson(userData, true);
            File.WriteAllText(path, json);
            //Debug.Log("Player data File created");
        }

        if (userData == null)
        {
            //Debug.Log("UserData is Null");
            return;
        }   

        
        json=JsonUtility.ToJson(userData,true);
        File.WriteAllText(path, json);
        //Debug.Log("Player data file Updatad");
    }

    public UserData LoadPlayerInfo()
    {
        //Debug.Log("LoadPlayerInfo Called");
        if (userData == null) Debug.Log("UserData is Null");
        if (File.Exists(path))
        {
            string loadedJson = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(loadedJson);
            //Debug.Log("Player Data Loaded");
        }
        return userData;
    }
}

[System.Serializable]
public class UserData
{
    public string playerName;
    public int playerScore;
    public bool isFirstTime = true;
    public bool isMemeAccepted = true;
    public int collectablesCount=0;
    public int currCharacterId=0;
}

