using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile_Button_Final : MonoBehaviour
{
    [Header("Profile Info")]
    public int profileID;

    [Header("UI")]
    public TMP_Text profileNameText;
    public Image accountStatusImage;

    [Header("Sprites")]
    public Sprite hasAccountSprite;
    public Sprite noAccountSprite;

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
            profileNameText.text = "Profile " + profileID;

            if (accountStatusImage != null && noAccountSprite != null)
            {
                accountStatusImage.sprite = noAccountSprite;
            }

            return;
        }

        if (Profile_Manager.Instance.TryGetProfileData(profileID, out Profile_Data data))
        {
            profileNameText.text = data.player_name;

            if (accountStatusImage != null && hasAccountSprite != null)
            {
                accountStatusImage.sprite = hasAccountSprite;
            }
        }
        else
        {
            profileNameText.text = "Empty Profile";

            if (accountStatusImage != null && noAccountSprite != null)
            {
                accountStatusImage.sprite = noAccountSprite;
            }
        }
    }
}