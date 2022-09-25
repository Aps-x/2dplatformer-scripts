using UnityEngine;
public class PlayerWallSlideState : PlayerBaseState
{
	bool canJump;
	public override void EnterState(Player player)
	{
		player.animator.SetBool("IsJumping", true);
		canJump = true;
	}
	public override void UpdateState(Player player)
	{
		// Left wall slide
		if (player.leftWallGrab)
		{
			player.spriteRenderer.flipX = false;
			if (player.jumpInput == 1 && canJump)
			{
				player.rb.velocity = Vector2.zero;
				player.rb.AddForce(Vector2.one * player.wallJumpForce, ForceMode2D.Impulse);
				canJump = false;
			}
		}
		// Right wall slide
		else if (player.rightWallGrab)
		{
			player.spriteRenderer.flipX = true;
			if (player.jumpInput == 1 && canJump)
			{
				player.rb.velocity = Vector2.zero;
				player.rb.AddForce(new Vector2(-1,1) * player.wallJumpForce, ForceMode2D.Impulse);
				canJump = false;
			}
		}
		else
		{
			ExitState(player, player.midairState);
		}
		// Player on ground
		if (player.onGround)
		{
			ExitState(player, player.idleState);
		}
		// Wall slide speed
		player.rb.velocity = new Vector2(player.rb.velocity.x, Mathf.Clamp(player.rb.velocity.y, -3, float.MaxValue));
	}
	public override void FixedUpdateState(Player player)
	{

	}
	public override void ExitState(Player player, PlayerBaseState state)
	{
		player.animator.SetBool("IsJumping", false);
		player.SwitchState(state);
	}
}