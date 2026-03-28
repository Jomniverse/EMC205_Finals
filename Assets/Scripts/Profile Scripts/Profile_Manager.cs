using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Profile_Manager : MonoBehaviour
{
    public static Profile_Manager Instance;

    [Header("Scene")]
    public string nextSceneName = "SampleScene";

    [Header("All Profile ScriptableObjects")]
    public Profile_Scriptable[] profiles;

    [Header("Current Profile")]
    public Profile_Scriptable currentProfile;

    [Header("Name Input UI")]
    public GameObject namePanel;
    public TMP_InputField nameInputField;

    private int pendingProfileID = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (namePanel != null)
        {
            namePanel.SetActive(false);
        }

        RefreshAllProfileButtons();
    }

    public void SelectProfile(int profileID)
    {
        Profile_Scriptable selectedProfile = GetProfileByID(profileID);

        if (selectedProfile == null)
        {
            Debug.LogError("No profile found with ID: " + profileID);
            return;
        }

        if (HasSaveFile(profileID))
        {
            currentProfile = selectedProfile;
            LoadProfile(profileID);

            Debug.Log("Loaded profile: " + currentProfile.player_name);

            if (!string.IsNullOrWhiteSpace(currentProfile.player_name))
            {
                GoToNextScene();
            }
        }
        else
        {
            pendingProfileID = profileID;

            if (namePanel != null)
            {
                namePanel.SetActive(true);
            }

            if (nameInputField != null)
            {
                nameInputField.text = "";
                nameInputField.ActivateInputField();
            }
        }
    }

    private void GoToNextScene()
    {
        if (currentProfile == null)
        {
            Debug.LogWarning("Cannot load next scene because no profile is selected.");
            return;
        }

        if (string.IsNullOrWhiteSpace(currentProfile.player_name))
        {
            Debug.LogWarning("Cannot load next scene because profile is not created yet.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    public void ConfirmNewProfileName()
    {
        if (pendingProfileID == -1)
        {
            Debug.LogWarning("No pending profile selected.");
            return;
        }

        if (nameInputField == null)
        {
            Debug.LogError("Name input field is missing.");
            return;
        }

        string enteredName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(enteredName))
        {
            Debug.LogWarning("Player name is empty.");
            return;
        }

        currentProfile = GetProfileByID(pendingProfileID);

        if (currentProfile == null)
        {
            Debug.LogError("Could not find selected profile.");
            return;
        }

        currentProfile.player_name = enteredName;
        currentProfile.player_money = 0;
        currentProfile.player_accuracy = 0f;
        currentProfile.player_experience = 0;

        SaveCurrentProfile();

        if (namePanel != null)
        {
            namePanel.SetActive(false);
        }

        if (nameInputField != null)
        {
            nameInputField.text = "";
        }

        pendingProfileID = -1;

        RefreshAllProfileButtons();

        GoToNextScene();
    }

    public void CancelNewProfileName()
    {
        pendingProfileID = -1;

        if (namePanel != null)
        {
            namePanel.SetActive(false);
        }

        if (nameInputField != null)
        {
            nameInputField.text = "";
        }
    }

    public void SaveCurrentProfile()
    {
        if (currentProfile == null)
        {
            Debug.LogWarning("No current profile selected.");
            return;
        }

        Profile_Data data = new Profile_Data();
        data.player_id = currentProfile.player_id;
        data.player_name = currentProfile.player_name;
        data.player_money = currentProfile.player_money;
        data.player_accuracy = currentProfile.player_accuracy;
        data.player_experience = currentProfile.player_experience;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetProfilePath(currentProfile.player_id), json);

        RefreshAllProfileButtons();
    }

    public void LoadProfile(int profileID)
    {
        string path = GetProfilePath(profileID);

        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file not found for profile " + profileID);
            return;
        }

        string json = File.ReadAllText(path);
        Profile_Data data = JsonUtility.FromJson<Profile_Data>(json);

        Profile_Scriptable profile = GetProfileByID(profileID);

        if (profile == null)
        {
            Debug.LogError("Profile ScriptableObject not found for ID " + profileID);
            return;
        }

        profile.player_id = data.player_id;
        profile.player_name = data.player_name;
        profile.player_money = data.player_money;
        profile.player_accuracy = data.player_accuracy;
        profile.player_experience = data.player_experience;

        currentProfile = profile;
    }

    public bool TryGetProfileData(int profileID, out Profile_Data data)
    {
        string path = GetProfilePath(profileID);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<Profile_Data>(json);
            return true;
        }

        data = null;
        return false;
    }

    public bool HasSaveFile(int profileID)
    {
        return File.Exists(GetProfilePath(profileID));
    }

    public Profile_Scriptable GetProfileByID(int id)
    {
        foreach (Profile_Scriptable profile in profiles)
        {
            if (profile.player_id == id)
            {
                return profile;
            }
        }

        return null;
    }

    private string GetProfilePath(int profileID)
    {
        return Path.Combine(Application.persistentDataPath, "profile_" + profileID + ".json");
    }

    public void RefreshAllProfileButtons()
    {
        Profile_Button[] allButtons = Object.FindObjectsByType<Profile_Button>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (Profile_Button button in allButtons)
        {
            button.RefreshDisplay();
        }
    }

    public void AddMoney(int amount)
    {
        if (currentProfile == null) return;

        currentProfile.player_money += amount;
        SaveCurrentProfile();
    }

    public void AddExperience(int amount)
    {
        if (currentProfile == null) return;

        currentProfile.player_experience += amount;
        SaveCurrentProfile();
    }

    public void SetAccuracy(float accuracy)
    {
        if (currentProfile == null) return;
                                                                
        currentProfile.player_accuracy = accuracy;
        SaveCurrentProfile();
    }


}