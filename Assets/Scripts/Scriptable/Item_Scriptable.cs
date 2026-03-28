using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create_Scriptable/Item")]
public class Item_Scriptable : ScriptableObject
{
    [Header("Item Info")]
    public int item_ID;

    public Sprite item_icon;

    public string item_name;

    public int item_price;

    [TextArea(3, 10)]
    public string item_description;
}