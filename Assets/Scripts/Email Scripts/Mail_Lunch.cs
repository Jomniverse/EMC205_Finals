using UnityEngine;
using UnityEngine.EventSystems;

public class Mail_Lunch : MonoBehaviour, IPointerClickHandler
{
    [Header("Existing panel in the scene")]
    [SerializeField] private GameObject targetPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount != 2) return;

        if (targetPanel != null)
        {
            targetPanel.SetActive(true);

        }
    }
}