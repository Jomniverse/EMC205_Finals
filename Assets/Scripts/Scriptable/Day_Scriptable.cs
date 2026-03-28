using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "Create_Scriptable/Create Day")]
public class Day_Scriptable : ScriptableObject
{
    public int dayNumber;
    public Caller_Scriptable[] callers;
}