using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.UIElements;
using UnityEngine;

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
        ItemSlot slot = items.Find(s => s.item == data);
        if (slot == null)
        {
            Debug.Log($"RemoveItem: {data.name} not found in inventory");
            return;
        }

        slot.quantity--;
        if (slot.quantity <= 0)
        {
            items.Remove(slot);
        }
        
        OnInventoryChanged?.Invoke();
    }
}
