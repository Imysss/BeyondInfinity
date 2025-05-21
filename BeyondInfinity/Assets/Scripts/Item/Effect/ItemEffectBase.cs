using UnityEngine;

public abstract class ItemEffectBase : ScriptableObject
{
	public Define.ConsumableType type;
	public float value;
    public abstract void Apply(PlayerController controller, PlayerCondition condition);
}
