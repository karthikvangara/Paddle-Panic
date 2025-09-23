using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;
    private string path;

    public void Awake()
    {
        instance = this;
        path = Application.persistentDataPath + "/UserData.json";
    }
    public void UpdatePlayerInfo(UserData userData)
    {
        string json;
        if (!File.Exists(path))
        {
            UserData defaultData = new UserData();
            defaultData.playerName = "Hi Bro";
            json = JsonUtility.ToJson(defaultData, true);
            File.WriteAllText(path, json);
            //Debug.Log("Player data File created");
        }

        if (userData == null)
        {
            Debug.Log("UserData is Null");
            return;
        }   

        
        json=JsonUtility.ToJson(userData,true);
        File.WriteAllText(path, json);
        //Debug.Log("Player data file Updatad");
    }

    public UserData LoadPlayerInfo()
    {
        UserData userData = new UserData();
        if (File.Exists(path))
        {
            string loadedJson = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(loadedJson);
            //Debug.Log("Player Data Loaded");
        }
        return userData;
    }
}
