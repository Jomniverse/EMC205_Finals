using UnityEngine;
using UnityEngine.UI;

public class Brought_Items : MonoBehaviour
{
    [SerializeField] private Image itemIcon;

    public void Setup(Item_Scriptable item)
    {
        if (item == null) return;

        if (itemIcon != null)
        {
            itemIcon.sprite = item.item_icon;
        }
    }
}