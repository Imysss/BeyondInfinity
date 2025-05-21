using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
[Serializable]

public class UI_Inventory : MonoBehaviour
{
    [Header("Prefab & Container")]
    public GameObject slotPrefab;
    private GameObject inventoryWindow;
    private Transform slotContainer;

    private List<ItemSlot> items;
    
    private Transform dropPosition;

    [Header("Select Item")] 
    private TextMeshProUGUI selectedItemNameText;
    private TextMeshProUGUI selectedItemDescriptionText;
    private TextMeshProUGUI selectedStatNameText;
    private TextMeshProUGUI selectedStatValueText;
    private GameObject useButton;
    private GameObject equipButton;
    private GameObject unequipButton;
    private GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;
    private PlayerInteraction interaction;
    private PlayerInventory inventory;

    private ItemData selectedItem;
    private int selectedItemIndex = 0;


    private void Start()
    {
        inventoryWindow = gameObject;
        slotContainer = transform.Find("ItemBackground/SlotContainer");

        dropPosition = PlayerManager.Instance.Player.interaction.dropPosition;
        
        //Select Item 연결
        selectedItemNameText = transform.Find("InfoBackground/ItemNameText").GetComponent<TextMeshProUGUI>();
        selectedItemDescriptionText = transform.Find("InfoBackground/ItemDescriptionText").GetComponent<TextMeshProUGUI>();
        selectedStatNameText = transform.Find("InfoBackground/StatNameText").GetComponent<TextMeshProUGUI>();
        selectedStatValueText = transform.Find("InfoBackground/StatValueText").GetComponent<TextMeshProUGUI>();
        useButton = transform.Find("InfoBackground/UseButton").gameObject;
        equipButton = transform.Find("InfoBackground/EquipButton").gameObject;
        unequipButton = transform.Find("InfoBackground/UnequipButton").gameObject;
        dropButton = transform.Find("InfoBackground/DropButton").gameObject;
        
        //player 연결
        controller = PlayerManager.Instance.Player.controller;
        condition = PlayerManager.Instance.Player.condition;
        interaction = PlayerManager.Instance.Player.interaction;
        inventory = PlayerManager.Instance.Player.inventory;
        
        //인벤토리 이벤트 연결
        controller.OnInventoryChanged += Toggle;
        inventory.OnInventoryChanged += RefreshUI;
        
        //인벤토리 UI 초기화
        inventoryWindow.SetActive(false);
        slotContainer.DestroyChildren();
        ClearSelectItemWindow();

        //버튼 이벤트 연결
        useButton.GetComponent<Button>().onClick.AddListener(OnUseButton);
        dropButton.GetComponent<Button>().onClick.AddListener(OnDropButton);
        //equipButton.GetComponent<Button>().onClick.AddListener(OnEquipButton);
        //unequipButton.GetComponent<Button>().onClick.AddListener(OnUnEquipButton);
    }

    private void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    private void ClearSelectItemWindow()
    {
        selectedItemNameText.text = string.Empty;
        selectedItemDescriptionText.text = string.Empty;
        selectedStatNameText.text = string.Empty;
        selectedStatValueText.text = string.Empty;
        
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    private bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    private void RefreshUI()
    {
        slotContainer.DestroyChildren();

        items = new List<ItemSlot>(inventory.items);

        foreach (var slot in items)
        {
            GameObject go = Instantiate(slotPrefab, slotContainer);
            UI_InventoryItem item = go.GetComponent<UI_InventoryItem>();
            item.Init(this);
            item.Set(slot.item, slot.quantity);
        }
    }

    public void SelectItem(ItemData data)
    {
        if (data == null) return;
        
        selectedItem = data;
        
        selectedItemNameText.text = selectedItem.name;
        selectedItemDescriptionText.text = selectedItem.description;
        selectedStatNameText.text = string.Empty;
        selectedStatValueText.text = string.Empty;
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatNameText.text += selectedItem.consumables[i].effect.type.ToString() + "\n";
            selectedStatValueText.text += selectedItem.consumables[i].effect.value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == Define.ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == Define.ItemType.Equipable);
        unequipButton.SetActive(selectedItem.type == Define.ItemType.Equipable);
        dropButton.SetActive(true);
    }

    private void OnUseButton()
    {
        if (selectedItem.type != Define.ItemType.Consumable)
            return;

        foreach (var consumable in selectedItem.consumables)
        {
            consumable.effect.Apply(controller, condition);
        }
        
        RemoveSelectedItem();
    }

    private void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    private void ThrowItem(ItemData data)
    {
        Instantiate(data.prefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    private void RemoveSelectedItem()
    {
        if (selectedItem == null) return;
        inventory.RemoveItem(selectedItem);
        ClearSelectItemWindow();
    }
}
