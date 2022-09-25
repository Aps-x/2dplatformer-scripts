using UnityEngine;
public class PlayerMidairState : PlayerBaseState
{
	float coyoteTime;
	float coyoteTimeInterval = 0.1f;
	public override void EnterState(Player player)
	{
		coyoteTime = Time.time + coyoteTimeInterval;
		player.rb.gravityScale = 2;
	}
	public override void UpdateState(Player player)
	{
		// Jumping sprite
		if (player.rb.velocity.y > 0)
		{
			player.animator.SetBool("IsJumping", true);
		}
		// Falling sprite
		else if (player.rb.velocity.y < 0)
		{
			player.animator.SetBool("IsJumping", false);
			player.animator.SetBool("IsFalling", true);
			if (player.jumpInput == 1 && coyoteTime > Time.time)
			{
				player.rb.gravityScale = 2;
				player.rb.velocity = Vector2.zero;
				player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
			}
		}
		// Wall grabbing
		if (player.leftWallGrab && player.rb.velocity.y < 0 || player.rightWallGrab && player.rb.velocity.y < 0)
		{
			ExitState(player, player.wallSlideState);
		}
		// Flip player sprite
		if (player.movement.x > 0)
		{
			player.spriteRenderer.flipX = false;
		}
		else if (player.movement.x < 0)
		{
			player.spriteRenderer.flipX = true;
		}
		// Gravity increase
		if (player.rb.velocity.y < 0 || player.jumpInput == 0)
		{
			player.rb.gravityScale = 4;
		}
		// Player grounded
		if (player.onGround)
		{
			ExitState(player, player.idleState);
		}
		// Player wants to climb ladder
		if (player.climbInput == 1 && player.onLadder && Time.time > player.ladderExitTime + 1f)
		{
			ExitState(player, player.ladderClimbState);
		}
	}
	public override void FixedUpdateState(Player player)
	{
		// Move the player
		player.rb.velocity = Vector2.Lerp(player.rb.velocity, new Vector2(player.movement.x * player.midairSpeed, player.rb.velocity.y), 0.1f);
	}
	public override void ExitState(Player player, PlayerBaseState state)
	{
		player.animator.SetBool("IsJumping", false);
		player.animator.SetBool("IsFalling", false);
		player.SwitchState(state);
	}
}