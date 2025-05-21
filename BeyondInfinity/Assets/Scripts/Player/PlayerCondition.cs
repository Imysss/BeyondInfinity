using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UI_Condition uiCondition;
    
    UI_ConditionItem health { get { return uiCondition.Health; } }
    UI_ConditionItem hunger { get { return uiCondition.Hunger; } }
    UI_ConditionItem stamina { get { return uiCondition.Stamina; } }

    private float _noHungerHealthDecay = 2f;    //배고픔이 0이 되면 줄어들 체력의 수

    public event Action OnTakeDamage;
    
    private void Update()
    {
        //hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        
        if (hunger.currentValue == 0)
        {
            health.Subtract(_noHungerHealthDecay * Time.deltaTime);
        }

        if (health.currentValue == 0)
        {
            OnDead();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void AddStamina(float amount)
    {
        stamina.Add(amount);
    }

    public bool SubtractStamina(float amount)
    {
        if (stamina.currentValue - amount < 0)
            return false;
        
        stamina.Subtract(amount);
        return true;
    }
    
    public void OnDamage(float damage)
    {
        health.Subtract(damage);
    }

    public void OnDead()
    {
        Debug.Log("Player Dead");
    }
}
