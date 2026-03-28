using UnityEngine;

[CreateAssetMenu(fileName = "Entries", menuName = "Create_Scriptable/Create Entries")]
public class Archive_Scriptable : ScriptableObject
{
    public string archive_name;
    public Sprite archive_image;

    [TextArea(5, 20)]
    public string description;
    [TextArea(5, 20)]
    public string risk;
    [TextArea(5, 20)]
    public string risk_bullets;
    [TextArea(5, 20)]
    public string solution;

    [Header("Unlock Settings")]
    public int unlockDay;

    public bool IsUnlocked(int currentDay)
    {
        return currentDay >= unlockDay;
    }
}