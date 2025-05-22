using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryItem : MonoBehaviour
{
    //Item Data
    private ItemData item;
    private bool equipped;
    private int quantity;
 
    //UI Components
    private Button button;
    private Image iconImage;
    private TextMeshProUGUI quantityText;
    private Outline outline;

    private UI_Inventory uiInventory;

    #region Initialization
    public void Init(UI_Inventory uiInventory)
    {
        button = button ?? GetComponent<Button>();
        iconImage = transform.Find("Icon").GetComponent<Image>();
        quantityText = transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
        outline = outline ?? GetComponent<Outline>();

        this.uiInventory = uiInventory;
        
        button.onClick.AddListener(OnClickButton);
    }
    #endregion

    #region UI Update Method
    public void RefreshUI()
    {
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
    #endregion

    #region UI Interaction
    private void OnClickButton()
    {
        uiInventory.SelectItem(item);
    }
    #endregion
}
