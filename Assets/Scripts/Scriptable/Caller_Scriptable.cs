using UnityEngine;

[CreateAssetMenu(fileName = "Caller", menuName = "Create_Scriptable/Create Caller")]
public class Caller_Scriptable : ScriptableObject
{
    public string caller_name;
    [TextArea(5, 10)] public string caller_dialogue;

    public Archive_Scriptable correct_answer;
}