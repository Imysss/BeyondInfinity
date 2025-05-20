using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnDamage(float damage);
}
public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UI_Condition uiCondition;
    
    Condition health { get { return uiCondition.Health; } }
    Condition hunger { get { return uiCondition.Hunger; } }
    Condition stamina { get { return uiCondition.Stamina; } }

    private float _noHungerHealthDecay = 2f;    //배고픔이 0이 되면 줄어들 체력의 수

    public event Action OnTakeDamage;
    
    private void Update()
    {
        hunger.Subtract((hunger.passiveValue * Time.deltaTime));
        //나중에 점프하거나 할 때 줄어들도록 설정
        stamina.Subtract((stamina.passiveValue * Time.deltaTime));

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
    
    public void OnDamage(float damage)
    {
        health.Subtract(damage);
    }

    public void OnDead()
    {
        Debug.Log("Player Dead");
    }
}
