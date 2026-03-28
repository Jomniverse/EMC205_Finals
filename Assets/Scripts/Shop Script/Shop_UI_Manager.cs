using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop_UI_Manager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject shopPanel;

    [Header("Shop Items")]
    [SerializeField] private Item_Scriptable[] shopItems;

    [Header("Spawned Item List")]
    [SerializeField] private Transform itemListParent;
    [SerializeField] private GameObject itemPrefab;

    [Header("Selected Item Info")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private TMP_Text itemPriceText;

    [Header("Money UI")]
    [SerializeField] private TMP_Text moneyText;

    [Header("Buy Button")]
    [SerializeField] private Button buyButton;

    private Item_Scriptable selectedItem;

    private void Awake()
    {
        if (shopPanel == null)
            shopPanel = gameObject;
    }

    private void OnEnable()
    {
        RefreshShopList();

        if (shopItems != null && shopItems.Length > 0 && shopItems[0] != null)
        {
            SelectItem(shopItems[0]);
        }

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuySelectedItem);
        }

        RefreshMoney();

        Shop_Manager.OnShopDataChanged += RefreshSelectedItemState;
        Shop_Manager.OnShopDataChanged += RefreshMoney;
    }

    private void OnDisable()
    {
        Shop_Manager.OnShopDataChanged -= RefreshSelectedItemState;
        Shop_Manager.OnShopDataChanged -= RefreshMoney;
    }

    public void RefreshShopList()
    {
        if (itemListParent == null || itemPrefab == null) return;

        for (int i = itemListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(itemListParent.GetChild(i).gameObject);
        }

        if (shopItems == null) return;

        for (int i = 0; i < shopItems.Length; i++)
        {
            if (shopItems[i] == null) continue;

            GameObject obj = Instantiate(itemPrefab, itemListParent);
            Shop_Item itemUI = obj.GetComponent<Shop_Item>();

            if (itemUI != null)
            {
                itemUI.Setup(shopItems[i], this);
            }
        }
    }

    public void SelectItem(Item_Scriptable item)
    {
        if (item == null) return;

        selectedItem = item;
        RefreshSelectedItemState();
    }

    private void RefreshSelectedItemState()
    {
        if (selectedItem == null) return;

        if (itemNameText != null)
            itemNameText.text = selectedItem.item_name;

        if (itemDescriptionText != null)
            itemDescriptionText.text = selectedItem.item_description;

        bool owned = false;

        if (Shop_Manager.Instance != null)
        {
            owned = Shop_Manager.Instance.CurrentPlayerHasItem(selectedItem.item_ID);
        }

        if (owned)
        {
            if (itemPriceText != null)
                itemPriceText.text = "Sold Out";

            if (buyButton != null)
                buyButton.interactable = false;
        }
        else
        {
            if (itemPriceText != null)
                itemPriceText.text = "Price: $" + selectedItem.item_price;

            if (buyButton != null)
                buyButton.interactable = true;
        }
    }

    public void RefreshMoney()
    {
        if (moneyText == null) return;
        if (Profile_Manager.Instance == null) return;
        if (Profile_Manager.Instance.currentProfile == null) return;

        moneyText.text = "Money: $" + Profile_Manager.Instance.currentProfile.player_money;
    }

    public void BuySelectedItem()
    {
        if (selectedItem == null) return;
        if (Shop_Manager.Instance == null) return;

        Shop_Manager.Instance.BuyItem(selectedItem);
    }

    public void OpenShopPanel()
    {
        if (shopPanel != null)
            shopPanel.SetActive(true);

        RefreshMoney();
    }
}