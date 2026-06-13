using UnityEngine;

public enum ItemType { DollGood, DollEvil, Fuel, Key }

[CreateAssetMenu(fileName = "NewItem", menuName = "WhisperingDolls/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public ItemType itemType;
}
