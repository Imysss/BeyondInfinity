using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] 
    public string name;
    public string description;
    public Define.ItemType type;
    public Sprite icon;
    public GameObject prefab;

    [Header("Stacking")] 
    public bool stackable;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
    
    [Header("Equip")] 
    public GameObject equipPrefab;
}
