using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketPlace_UI_Manager : MonoBehaviour
{

    [Header("Top Money Text")]
    [SerializeField] private TMP_Text moneyText;

    [Header("Tabs")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject transactionPanel;

    [Header("Transaction Items")]
    [SerializeField] private Item_Scriptable[] allItems;
    [SerializeField] private Transform transactionParent;
    [SerializeField] private GameObject transactionItemPrefab;

    private void OnEnable()
    {
        RefreshMoney();
        ShowShopTab();
    }

    public void ShowShopTab()
    {
        if (shopPanel != null)
            shopPanel.SetActive(true);

        if (transactionPanel != null)
            transactionPanel.SetActive(false);

        RefreshMoney();
    }

    public void ShowTransactionTab()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        if (transactionPanel != null)
            transactionPanel.SetActive(true);

        RefreshMoney();
        RefreshTransactionItems();
    }

    public void RefreshMoney()
    {
        if (moneyText == null) return;
        if (Profile_Manager.Instance == null) return;
        if (Profile_Manager.Instance.currentProfile == null) return;

        moneyText.text = "Money: $" + Profile_Manager.Instance.currentProfile.player_money;
    }

    public void RefreshTransactionItems()
    {
        if (transactionParent == null || transactionItemPrefab == null) return;
        if (Shop_Manager.Instance == null) return;

        for (int i = transactionParent.childCount - 1; i >= 0; i--)
        {
            Destroy(transactionParent.GetChild(i).gameObject);
        }

        List<int> ownedItemIDs = Shop_Manager.Instance.GetCurrentPlayerOwnedItemIDs();

        for (int i = 0; i < ownedItemIDs.Count; i++)
        {
            Item_Scriptable foundItem = FindItemByID(ownedItemIDs[i]);

            if (foundItem == null) continue;

            GameObject obj = Instantiate(transactionItemPrefab, transactionParent);
            Transaction_Item transactionUI = obj.GetComponent<Transaction_Item>();

            if (transactionUI != null)
            {
                transactionUI.Setup(foundItem);
            }
        }
    }

    private Item_Scriptable FindItemByID(int itemID)
    {
        if (allItems == null) return null;

        for (int i = 0; i < allItems.Length; i++)
        {
            if (allItems[i] == null) continue;

            if (allItems[i].item_ID == itemID)
            {
                return allItems[i];
            }
        }

        return null;
    }
}