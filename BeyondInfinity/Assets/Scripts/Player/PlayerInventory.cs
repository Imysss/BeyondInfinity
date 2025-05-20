using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.UIElements;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public ItemData item;
    public int quantity;

    public ItemSlot(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
public class PlayerInventory : MonoBehaviour
{
    public Action OnInventoryChanged;

    public List<ItemSlot> items = new List<ItemSlot>();

    public void AddItem(ItemData data)
    {
        ItemSlot slot = items.Find(slot =>
            slot.item == data && data.stackable && slot.quantity < data.maxStackAmount);
        if (slot != null)
        {
            slot.quantity++;
        }
        else
        {
            items.Add(new ItemSlot(data, 1));
        }
        
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData data)
    {
        OnInventoryChanged?.Invoke();
    }
}
