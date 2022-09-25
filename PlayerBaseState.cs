using UnityEngine;
public abstract class PlayerBaseState
{
	public abstract void EnterState(Player player);
	public abstract void UpdateState(Player player);
	public abstract void FixedUpdateState(Player player);
	public abstract void ExitState(Player player, PlayerBaseState state);
}