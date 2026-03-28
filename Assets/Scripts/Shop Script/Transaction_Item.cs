using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transaction_Item : MonoBehaviour
{
    [SerializeField] private TMP_Text productNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image itemIcon;

    public void Setup(Item_Scriptable item)
    {
        if (item == null) return;

        if (productNameText != null)
            productNameText.text = item.item_name;

        if (priceText != null)
            priceText.text = "Price: " + item.item_price;

        if (itemIcon != null)
            itemIcon.sprite = item.item_icon;
    }
}