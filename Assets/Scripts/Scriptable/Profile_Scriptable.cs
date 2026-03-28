using UnityEngine;

[CreateAssetMenu(fileName = "Profile", menuName = "Create_Scriptable/Create Profile")]
public class Profile_Scriptable : ScriptableObject
{
    [Header("Basic Info")]
    public int player_id;
    public string player_name;

    public int player_money;
    public float player_accuracy;
    public int player_experience;
}

