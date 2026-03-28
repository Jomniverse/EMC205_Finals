using UnityEngine;
using UnityEngine.EventSystems;

public class Profile_Launch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Report_Manager report_manager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount != 2) return;


        report_manager.OpenReportPanel();
    
    }
}