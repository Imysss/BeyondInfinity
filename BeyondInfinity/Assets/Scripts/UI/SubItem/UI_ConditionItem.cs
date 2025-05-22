using UnityEngine;
using UnityEngine.UI;

public class UI_ConditionItem : MonoBehaviour
{
    //Value Fields
    public float currentValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;

    //UI Elements
    public Image uiBar;

    #region Unity Methods
    private void Start()
    {
        currentValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }
    #endregion

    #region Value Modification
    public void Add(float value)
    {
        currentValue = Mathf.Min(currentValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        currentValue = Mathf.Max(currentValue - value, 0);
    }
    #endregion

    #region Utility
    private float GetPercentage()
    {
        return currentValue / maxValue;
    }
    #endregion
}
