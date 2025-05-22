using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Eat")]
public class EatEffect : ItemEffectBase
{
    public override void Apply(PlayerController controller, PlayerCondition condition)
    {
        condition.Eat(value);
    }
}
