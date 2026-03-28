using UnityEngine;

[CreateAssetMenu(fileName = "DayMail", menuName = "Create_Scriptable/Create Day Mail")]
public class DayMail_Scriptable : ScriptableObject
{
    public int dayNumber = 1;
    public Mail_Scriptable[] mails;
}