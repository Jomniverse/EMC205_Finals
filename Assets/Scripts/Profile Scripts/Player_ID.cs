using TMPro;
using UnityEngine;

public class PlayerID_UI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerIDText;

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
        if (Profile_Manager.Instance != null)
        {
            playerIDText.text = "Player ID: " + Profile_Manager.Instance.currentProfile;
        }
        else
        {
            playerIDText.text = "Player ID: -";
        }
    }
}