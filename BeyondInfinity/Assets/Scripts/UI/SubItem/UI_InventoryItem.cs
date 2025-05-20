using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryItem : MonoBehaviour
{
    private ItemData item;
    public ItemData Item { get { return item; } }

    private int index;
    private bool equipped;
    private int quantity;
    public int Quantity { get { return quantity; } set { quantity = value; } }
    
    private Button button;
    private Image iconImage;
    private TextMeshProUGUI quantityText;
    private Outline outline;

    private UI_Inventory uiInventory;

    private void Awake()
    {
        button = GetComponent<Button>();
        iconImage = GetComponentInChildren<Image>();
        quantityText = GetComponentInChildren<TextMeshProUGUI>();
        outline = GetComponent<Outline>();
        
        uiInventory = GetComponentInParent<UI_Inventory>();
        
        button.onClick.AddListener(OnClickButton);
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Init()
    {
        iconImage.gameObject.SetActive(true);
        iconImage.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        item = null;
        iconImage.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public bool IsStackable(int amount)
    {
        if (quantity < amount)
            return true;
        return false;
    }

    private void OnClickButton()
    {
        //uiInventory.SelectItem(index);
    }
}
