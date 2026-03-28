using System.Collections.Generic;

[System.Serializable]
public class Shop_SaveData
{
    public List<Player_Inventory> inventory = new List<Player_Inventory>();
    public List<Player_Transaction> transactionLogs = new List<Player_Transaction>();
}