using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/AddSpeed")]
public class AddSpeedEffect : ItemEffectBase
{
    public override void Apply(PlayerController controller, PlayerCondition condition)
    {
        controller.AddSpeed(value);
    }
}
