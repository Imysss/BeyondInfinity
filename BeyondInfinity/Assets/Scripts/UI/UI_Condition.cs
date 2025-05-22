using UnityEngine;

public class UI_Condition : MonoBehaviour
{
    private UI_ConditionItem _health;
    public UI_ConditionItem Health { get => _health; }
    private UI_ConditionItem _hunger;
    public UI_ConditionItem Hunger { get => _hunger; }
    private UI_ConditionItem _stamina;
    public UI_ConditionItem Stamina { get => _stamina; }

    private void Start()
    {
        PlayerManager.Instance.Player.condition.uiCondition = this;
        
        _health = transform.Find("Health")?.GetComponent<UI_ConditionItem>();
        _hunger = transform.Find("Hunger")?.GetComponent<UI_ConditionItem>();
        _stamina = transform.Find("Stamina")?.GetComponent<UI_ConditionItem>();
    }
}
