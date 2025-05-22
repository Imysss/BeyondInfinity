using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/EnableDoubleJump")]
public class EnableDoubleJumpEffect : ItemEffectBase
{
    public override void Apply(PlayerController controller, PlayerCondition condition)
    {
        controller.DoubleJump();
    }
}
