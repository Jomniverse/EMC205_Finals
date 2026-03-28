using UnityEngine;

public class Close_Panel : MonoBehaviour
{
    [SerializeField] private GameObject targetPanel;

    public void CloseThisPanel()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }
    }
}