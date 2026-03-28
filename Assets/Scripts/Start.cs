using UnityEngine;
using UnityEngine.EventSystems;

public class Start : MonoBehaviour
{
    public GameObject panel;
    public void Hide()
    {
        panel.SetActive(false);
    }
}