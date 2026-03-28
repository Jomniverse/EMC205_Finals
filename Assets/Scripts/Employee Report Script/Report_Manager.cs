using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Report_Manager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject reportPanel;

    [Header("Player Info")]
    [SerializeField] private TMP_Text profileNameText;
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private TMP_Text accuracyText;
    [SerializeField] private TMP_Text moneyText;

    [SerializeField] private TMP_Text idText;


    [Header("Bought Items")]
    [SerializeField] private Item_Scriptable[] allItems;
    [SerializeField] private Transform boughtItemParent;
    [SerializeField] private GameObject boughtItemPrefab;

    private void Awake()
    {
        if (reportPanel == null)
            reportPanel = gameObject;
    }

    private void OnEnable()
    {
        RefreshReport();
        Shop_Manager.OnShopDataChanged += RefreshReport;
    }

    private void OnDisable()
    {
        Shop_Manager.OnShopDataChanged -= RefreshReport;
    }

    public void OpenReportPanel()
    {
        if (reportPanel != null)
            reportPanel.SetActive(true);

        RefreshReport();
        RefreshMoney();
    }

    public void CloseReportPanel()
    {
        if (reportPanel != null)
            reportPanel.SetActive(false);
    }

    public void RefreshReport()
    {
        RefreshPlayerInfo();
        RefreshBoughtItems();
    }

    private void RefreshPlayerInfo()
    {
        if (Profile_Manager.Instance == null || Profile_Manager.Instance.currentProfile == null) return;

        Profile_Scriptable profile = Profile_Manager.Instance.currentProfile;

        if (profileNameText != null)
            profileNameText.text = profile.player_name;

        if (idText != null)
            idText.text = "Player ID: " + profile.player_id.ToString();

        if (experienceText != null)
            experienceText.text = "Experience: " + profile.player_experience;

        if (accuracyText != null)                                                   
            accuracyText.text = "Accuracy: " + profile.player_accuracy;

        RefreshMoney();
    }

    public void RefreshMoney()
    {
        if (moneyText == null) return;
        if (Profile_Manager.Instance == null) return;
        if (Profile_Manager.Instance.currentProfile == null) return;

        moneyText.text = "Money $: " + Profile_Manager.Instance.currentProfile.player_money;
    }

    private void RefreshBoughtItems()
    {
        if (boughtItemParent == null || boughtItemPrefab == null) return;
        if (Shop_Manager.Instance == null) return;

        for (int i = boughtItemParent.childCount - 1; i >= 0; i--)
        {
            Destroy(boughtItemParent.GetChild(i).gameObject);
        }

        List<int> ownedItemIDs = Shop_Manager.Instance.GetCurrentPlayerOwnedItemIDs();

        for (int i = 0; i < ownedItemIDs.Count; i++)
        {
            Item_Scriptable foundItem = FindItemByID(ownedItemIDs[i]);

            if (foundItem == null) continue;

            GameObject obj = Instantiate(boughtItemPrefab, boughtItemParent);
            Brought_Items boughtItemUI = obj.GetComponent<Brought_Items>();

            if (boughtItemUI != null)
            {
                boughtItemUI.Setup(foundItem);
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