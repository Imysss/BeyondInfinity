using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Heal")]
public class HealEffect : ItemEffectBase
{
    public override void Apply(PlayerController controller, PlayerCondition condition)
    {
        condition.Heal(value);
    }
}
