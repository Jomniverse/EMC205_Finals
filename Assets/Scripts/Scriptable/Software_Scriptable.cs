using UnityEngine;

[CreateAssetMenu(fileName = "SoftWare", menuName = "Create_Scriptable/Create Software")]
public class Software_Scriptable : ScriptableObject
{
    [Header("Basic Info")]
    public string software_name;
    [TextArea] public string software_description;

    [Header("Icon")]
    public Sprite software_icon;

}



