using UnityEngine;
using UnityEngine.UI;

public class Shop_Item : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button button;

    private Item_Scriptable currentItem;
    private Shop_UI_Manager shopUIManager;

    public void Setup(Item_Scriptable item, Shop_UI_Manager manager)
    {
        currentItem = item;
        shopUIManager = manager;

        if (itemIcon != null)
        {
            itemIcon.sprite = item.item_icon;
        }

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClickItem);
        }
    }

    private void OnClickItem()
    {
        if (shopUIManager != null && currentItem != null)
        {
            shopUIManager.SelectItem(currentItem);
        }
    }
}