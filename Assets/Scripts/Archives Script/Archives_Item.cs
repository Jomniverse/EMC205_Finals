using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Archives_Item : MonoBehaviour
{
    [SerializeField] private TMP_Text archive_name;
    [SerializeField] private Button button;

    private Archive_Scriptable current_Archive;
    private Archive_Manager archive_Manager;

    public void Setup(Archive_Scriptable archiveData, Archive_Manager manager)
    {
        archive_Manager = manager;

        bool isUnlocked = archiveData != null && manager != null && manager.IsArchiveUnlocked(archiveData);

        current_Archive = isUnlocked ? archiveData : null;

        if (archive_name != null)
            archive_name.text = isUnlocked ? archiveData.archive_name : "ENTRY INACCESSIBLE";

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.interactable = isUnlocked;

            if (isUnlocked)
                button.onClick.AddListener(OnClickArchive);
        }
    }

    public void OnClickArchive()
    {
        if (archive_Manager != null && current_Archive != null)
            archive_Manager.ShowArchive(current_Archive);
    }
}