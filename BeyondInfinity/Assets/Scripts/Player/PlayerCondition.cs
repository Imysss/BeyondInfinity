using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    //UI Binding
    public UI_Condition uiCondition;
    UI_ConditionItem health { get { return uiCondition.Health; } }
    UI_ConditionItem hunger { get { return uiCondition.Hunger; } }
    UI_ConditionItem stamina { get { return uiCondition.Stamina; } }

    //Config
    private float _noHungerHealthDecay = 2f;    //배고픔이 0이 되면 줄어들 체력의 수

    //Events
    public event Action OnTakeDamage;

    #region Unity Methods
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
    #endregion

    #region Condition Methods
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
    #endregion
    
    public void OnDead()
    {
        Debug.Log("Player Dead");
    }
}
