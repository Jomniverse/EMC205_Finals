using UnityEngine;
using UnityEngine.EventSystems;

public class Shop_Launch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Shop_UI_Manager shop_manager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount != 2) return;


        shop_manager.OpenShopPanel();

    }
}