using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryItem : MonoBehaviour
{
    private ItemData item;
    private int index;
    private bool equipped;
    private int quantity;
 
    private Button button;
    private Image iconImage;
    private TextMeshProUGUI quantityText;
    private Outline outline;

    private UI_Inventory uiInventory;

    public void Init()
    {
        button = button ?? GetComponent<Button>();
        iconImage = transform.Find("Icon").GetComponent<Image>();
        quantityText = transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
        outline = outline ?? GetComponent<Outline>();
        
        uiInventory = GetComponentInParent<UI_Inventory>();
        
        button.onClick.AddListener(OnClickButton);
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void RefreshUI()
    {
        Debug.Log($"[Init] iconImage: {iconImage}, item: {item}");
        if (iconImage == null)
        {
            Debug.Log("iconImage가 null입니다!");
        }
        iconImage.gameObject.SetActive(true);
        iconImage.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Set(ItemData data, int quantity)
    {
        this.item = data;
        this.quantity = quantity;

        RefreshUI();
    }

    public void Clear()
    {
        item = null;
        iconImage.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }
    
    private void OnClickButton()
    {
        //uiInventory.SelectItem(index);
    }
}
