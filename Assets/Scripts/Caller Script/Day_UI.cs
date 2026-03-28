using TMPro;
using UnityEngine;

public class Day_UI : MonoBehaviour
{
    [SerializeField] private Call_Manager call_manager;
    public TMP_Text dayText;

    private void OnEnable()
    {
        RefreshUI();
    }

    private void Update()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        dayText.text = "Day: " + Call_Manager.Instance.current_day_counter.ToString();

    }
}