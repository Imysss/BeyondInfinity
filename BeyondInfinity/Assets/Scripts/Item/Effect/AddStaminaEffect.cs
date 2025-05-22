using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/AddStamina")]
public class AddStaminaEffect : ItemEffectBase
{
    public override void Apply(PlayerController controller, PlayerCondition condition)
    {
        condition.AddStamina(value);
    }
}
