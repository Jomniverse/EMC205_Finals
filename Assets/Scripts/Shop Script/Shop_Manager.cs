using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Shop_Manager : MonoBehaviour
{
    public static Shop_Manager Instance;

    public static event Action OnShopDataChanged;

    private Shop_SaveData saveData = new Shop_SaveData();

    private string SavePath => Path.Combine(Application.persistentDataPath, "shop.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private int GetCurrentPlayerID()
    {
        if (Profile_Manager.Instance == null || Profile_Manager.Instance.currentProfile == null)
        {
            Debug.LogError("No current profile selected.");
            return -1;
        }

        return Profile_Manager.Instance.currentProfile.player_id;
    }

    public bool CurrentPlayerHasItem(int itemID)
    {
        int currentPlayerID = GetCurrentPlayerID();
        if (currentPlayerID == -1) return false;

        foreach (Player_Inventory entry in saveData.inventory)
        {
            if (entry.playerID == currentPlayerID && entry.itemID == itemID)
            {
                return true;
            }
        }

        return false;
    }

    public bool BuyItem(Item_Scriptable item)
    {
        if (item == null)
        {
            Debug.LogWarning("Item is null.");
            return false;
        }

        if (Profile_Manager.Instance == null || Profile_Manager.Instance.currentProfile == null)
        {
            Debug.LogWarning("No current profile selected.");
            return false;
        }

        int currentPlayerID = GetCurrentPlayerID();
        if (currentPlayerID == -1) return false;

        if (CurrentPlayerHasItem(item.item_ID))
        {
            Debug.Log("Item already owned by current player.");
            return false;
        }

        int playerCurrency = Profile_Manager.Instance.currentProfile.player_money;
        int itemPrice = item.item_price;

        if (playerCurrency < itemPrice)
        {
            Debug.Log("Purchase failed: not enough money.");
            return false;
        }

        Profile_Manager.Instance.currentProfile.player_money -= itemPrice;

        saveData.inventory.Add(new Player_Inventory
        {
            playerID = currentPlayerID,
            itemID = item.item_ID
        });

        saveData.transactionLogs.Add(new Player_Transaction
        {
            playerID = currentPlayerID,
            itemID = item.item_ID,
            price = itemPrice
        });

        Profile_Manager.Instance.SaveCurrentProfile();
        Save();

        OnShopDataChanged?.Invoke();

        Debug.Log("Purchased: " + item.item_name);
        return true;
    }

    public List<int> GetCurrentPlayerOwnedItemIDs()
    {
        List<int> ownedItemIDs = new List<int>();

        int currentPlayerID = GetCurrentPlayerID();
        if (currentPlayerID == -1) return ownedItemIDs;

        foreach (Player_Inventory entry in saveData.inventory)
        {
            if (entry.playerID == currentPlayerID)
            {
                ownedItemIDs.Add(entry.itemID);
            }
        }

        return ownedItemIDs;
    }

    public List<Player_Transaction> GetCurrentPlayerPurchaseLogs()
    {
        List<Player_Transaction> currentPlayerLogs = new List<Player_Transaction>();

        int currentPlayerID = GetCurrentPlayerID();
        if (currentPlayerID == -1) return currentPlayerLogs;

        foreach (Player_Transaction log in saveData.transactionLogs)
        {
            if (log.playerID == currentPlayerID)
            {
                currentPlayerLogs.Add(log);
            }
        }

        return currentPlayerLogs;
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
    }

    private void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            saveData = JsonUtility.FromJson<Shop_SaveData>(json);
        }
        else
        {
            saveData = new Shop_SaveData();
        }
    }
}