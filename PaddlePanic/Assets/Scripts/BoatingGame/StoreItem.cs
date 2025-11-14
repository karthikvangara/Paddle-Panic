using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public string itemName;
    public int itemCost;
    public int itemId;
    public bool isPurchased;
    public bool isSelected;

    public TMP_Text itemNamePanel;
    public TMP_Text itemCostPanel;

    public GameObject buy;
    public GameObject select;
    public GameObject selected;

    public void UpdateItemInfo()
    {
        itemNamePanel.text = itemName;
        itemCostPanel.text = itemCost.ToString();
        CheckForPurchase();
    }

    public void CheckForPurchase()
    {
        DisableAllOptions();
        if (isPurchased)
        {
            UserData userData = UserDataManager.instance.LoadPlayerInfo();
            if (userData.currBoatId == itemId)
            {
                isSelected = true;
                selected.SetActive(true);
            }
            else select.SetActive(true);
        }
        else buy.SetActive(true);
    }

    public void OnClickBuy()
    {
        ProcessPayment();
    }

    public void OnClickSelectItem()
    {
        DisableAllOptions();
        selected.SetActive(true);
        isSelected = true;
        UserData userData = new UserData();
        userData = UserDataManager.instance.LoadPlayerInfo();
        userData.currBoatId = itemId;
        UserDataManager.instance.UpdatePlayerInfo(userData);
        UpdateStoreData();
        StoreManager.instance.LoadStoreItemsInGame();
    }

    public void OnClickDeselectItem()
    {
        DisableAllOptions();
        if(isPurchased) select.SetActive(true); 
        else buy.SetActive(true);
        isSelected =false;
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
            isPurchased = true;
            DisableAllOptions();
            select.SetActive(true);

            userData.collectablesCount -= itemCost;
            UserDataManager.instance.UpdatePlayerInfo(userData);
            UpdateStoreData();
        }
        else
        {
            Debug.Log("Insufficient Funds");
        }
    }

    public void UpdateStoreData()
    { 
        for(int i=0;i< StoreManager.instance.storeData.storeItemInfos.Count;i++)
        {
            if (itemId == StoreManager.instance.storeData.storeItemInfos[i].itemId)
            {
                StoreManager.instance.storeData.storeItemInfos[i].isPurchased = isPurchased;
                StoreManager.instance.storeData.storeItemInfos[i].isSelected = isSelected;
            }
        }
    }

}
