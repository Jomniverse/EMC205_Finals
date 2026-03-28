using UnityEngine;

[CreateAssetMenu(fileName = "Mail", menuName = "Create_Scriptable/Create Mail")]
public class Mail_Scriptable : ScriptableObject
{
    public string sender;
    public string subject;

    [TextArea(5, 20)]
    public string message;
}