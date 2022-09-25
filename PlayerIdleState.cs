using UnityEngine;
public class PlayerIdleState : PlayerBaseState
{
	bool canJump;
	public override void EnterState(Player player)
	{
		player.rb.velocity = Vector2.zero;
		player.rb.gravityScale = 2;
		canJump = true;
	}
	public override void UpdateState(Player player)
	{
		// Player wants to crouch
		if (player.crouchInput == 1)
		{
			ExitState(player, player.crouchState);
		}
		// Player starts moving
		if (Mathf.Abs(player.movement.x) > 0.5f)
		{
			ExitState(player, player.runningState);
		}
		// Player jumps
		if (player.jumpInput == 1 && player.onGround && canJump)
		{
			player.rb.velocity = Vector2.zero;
			player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
			canJump = false;
		}
		// Player leaves the ground
		if (!player.onGround)
		{
			ExitState(player, player.midairState);
		}
		// Player wants to climb ladder
		if (player.climbInput == 1 && player.onLadder && Time.time > player.ladderExitTime + 1f)
		{
			ExitState(player, player.ladderClimbState);
		}
		// Stop horizontal platforms causing players to slide
		if (!player.onPlatform && !player.onConveyorBelt && player.jumpInput != 1)
		{
			player.rb.velocity *= new Vector2(0, 1);
		}
	}
	public override void FixedUpdateState(Player player)
	{

	}
	public override void ExitState(Player player, PlayerBaseState state)
	{
		player.SwitchState(state);
	}
}