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
    public float paddleForce;

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
            UserDataManager.instance.LoadPlayerInfo();
            if (UserDataManager.instance.userData.currCharacterId == itemId)
            {
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
        UserDataManager.instance.LoadPlayerInfo();
        UserDataManager.instance.userData.currCharacterId = itemId;
        UserDataManager.instance.UpdatePlayerInfo();
        StoreManager.instance.UpdateStoreInfoData(itemId, isPurchased, isSelected);
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
        UserDataManager.instance.LoadPlayerInfo();
        if (UserDataManager.instance.userData.collectablesCount >= itemCost)
        {
            isPurchased = true;
            DisableAllOptions();
            select.SetActive(true);

            UserDataManager.instance.userData.collectablesCount -= itemCost;
            UserDataManager.instance.UpdatePlayerInfo();
            StoreManager.instance.UpdateStoreInfoData(itemId,isPurchased,isSelected);   
        }
        else
        {
            Debug.Log("Insufficient Funds");
        }
    }
}
