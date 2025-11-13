using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public string itemName;
    public int itemCost;
    public int itemId;
    public bool isLocked;

    public GameObject buy;
    public GameObject select;
    public GameObject selected;

    public void Buy()
    {
        ProcessPayment();
    }

    public void SelectItem()
    {
        DisableAllOptions();
        selected.SetActive(true);

        UserData userData = new UserData();
        userData = UserDataManager.instance.LoadPlayerInfo();
        userData.currBoatId = itemId;
        UserDataManager.instance.UpdatePlayerInfo(userData);
    }

    public void DeselectItem()
    {
        DisableAllOptions();
        if(isLocked) buy.SetActive(true);
        else select.SetActive(true);
    }

    public void DisableAllOptions()
    {
        buy.SetActive(false);
        select.SetActive(false);
        selected.SetActive(false);
    }

    public void ProcessPayment()
    {
        UserData userData = new UserData();
        userData = UserDataManager.instance.LoadPlayerInfo();
        if (userData.collectablesCount >= itemCost)
        {
            isLocked = false;
            DisableAllOptions();
            select.SetActive(true);

            userData.collectablesCount -= itemCost;
            UserDataManager.instance.UpdatePlayerInfo(userData);
        }
        else
        {
            Debug.Log("Insufficient Funds");
        }
    }

}
