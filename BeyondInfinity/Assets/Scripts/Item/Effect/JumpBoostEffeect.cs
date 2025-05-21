using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/JumpBoost")]
public class JumpBoostEffeect : ItemEffectBase
{
    public override void Apply(PlayerController controller, PlayerCondition condition)
    {
        controller.AddJumpPower(value);
    }
}
