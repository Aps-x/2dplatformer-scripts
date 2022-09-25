using UnityEngine;
public class PlayerRunningState : PlayerBaseState
{
	bool canJump;
	public override void EnterState(Player player)
	{
		canJump = true;
		player.animator.SetBool("IsRunning", true);
	}
	public override void UpdateState(Player player)
	{
		// Flip Player Sprite
		if (player.movement.x > 0)
		{
			player.spriteRenderer.flipX = false;
		}
		else if (player.movement.x < 0)
		{
			player.spriteRenderer.flipX = true;
		}
		// Player stops moving
		if (Mathf.Abs(player.movement.x) < 0.5f)
		{
			ExitState(player, player.idleState);
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
		// Player wants to crouch
		if (player.crouchInput == 1)
		{
			ExitState(player, player.crouchState);
		}
	}
	public override void FixedUpdateState(Player player)
	{
		// Move the player
		if (player.conveyor != null)
		{
			player.rb.velocity = new Vector2((player.movement.x * player.playerSpeed) + ((-player.conveyor.conveyorDirection.x * player.conveyor.speed)*0.5f), player.rb.velocity.y);
		}
		else
		{
			player.rb.velocity = new Vector2(player.movement.x * player.playerSpeed, player.rb.velocity.y);
		}
	}
	public override void ExitState(Player player, PlayerBaseState state)
	{
		player.animator.SetBool("IsRunning", false);
		player.SwitchState(state);
	}
}