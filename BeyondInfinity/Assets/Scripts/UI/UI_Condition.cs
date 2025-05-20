using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Condition : MonoBehaviour
{
    private Condition _health;
    public Condition Health { get => _health; }
    private Condition _hunger;
    public Condition Hunger { get => _hunger; }
    private Condition _stamina;
    public Condition Stamina { get => _stamina; }

    private void Start()
    {
        PlayerManager.Instance.Player.condition.uiCondition = this;
        
        _health = transform.Find("Health")?.GetComponent<Condition>();
        _hunger = transform.Find("Hunger")?.GetComponent<Condition>();
        _stamina = transform.Find("Stamina")?.GetComponent<Condition>();
    }
}
