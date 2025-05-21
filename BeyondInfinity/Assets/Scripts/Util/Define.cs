using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum ItemType
    {
        Equipable,
        Consumable,
    }

    public enum ConsumableType
    {
        Hunger,
        Health,
        Stamina,
        Speed,
        JumpPower,
        DoubleJump,
    }
}

[Serializable]
public class ItemDataConsumable
{
    public ItemEffectBase effect;
}

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
