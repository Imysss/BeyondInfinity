using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float currentValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;

    public Image uiBar;

    private void Start()
    {
        currentValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return currentValue / maxValue;
    }

    public void Add(float value)
    {
        currentValue = Mathf.Min(currentValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        currentValue = Mathf.Max(currentValue - value, 0);
    }
}
