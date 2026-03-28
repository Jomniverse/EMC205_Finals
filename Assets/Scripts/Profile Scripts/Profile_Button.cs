using TMPro;
using UnityEngine;

public class Profile_Button : MonoBehaviour
{
    [Header("Profile Info")]
    public int profileID;

    [Header("UI Texts")]
    public TMP_Text profileNameText;
    public TMP_Text experienceText;
    public TMP_Text moneyText;
    public TMP_Text accuracyText;

    private void Start()
    {
        RefreshDisplay();
    }

    private void OnEnable()
    {
        RefreshDisplay();
    }

    public void ChooseProfile()
    {
        if (Profile_Manager.Instance != null)
        {
            Profile_Manager.Instance.SelectProfile(profileID);
        }
    }

    public void RefreshDisplay()
    {
        if (Profile_Manager.Instance == null)
        {
            return;
        }

        if (Profile_Manager.Instance.TryGetProfileData(profileID, out Profile_Data data))
        {
            profileNameText.text = data.player_name;
            experienceText.text = "Experience: " + data.player_experience;
            moneyText.text = "Money: " + data.player_money;
            accuracyText.text = "Accuracy: " + data.player_accuracy;
        }
        else
        {
            profileNameText.text = "Profile " + profileID;
            experienceText.text = "";
            moneyText.text = "";
            accuracyText.text = "";
        }
    }
}