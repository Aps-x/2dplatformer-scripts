using UnityEngine;
public class PlayerDeathState : PlayerBaseState
{
	float timer = 0;
	public override void EnterState(Player player)
	{
		player.animator.SetBool("IsDead", true);
		player.rb.velocity = Vector2.zero;
		player.rb.gravityScale = 0;
	}
	public override void UpdateState(Player player)
	{
		timer += Time.deltaTime;
		if (timer > 2.5f)
		{
			ExitState(player, player.idleState);
			timer = 0;
		}
	}
	public override void FixedUpdateState(Player player)
	{

	}
	public override void ExitState(Player player, PlayerBaseState state)
	{
		player.animator.SetBool("IsDead", false);
		player.SwitchState(state);
	}
}