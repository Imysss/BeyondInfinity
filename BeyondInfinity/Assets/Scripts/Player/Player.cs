using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public PlayerInteraction interaction;
    public PlayerInventory inventory;
    
    public Item[] items;
    
    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        interaction = GetComponent<PlayerInteraction>();
        inventory = GetComponent<PlayerInventory>();
    }
}
