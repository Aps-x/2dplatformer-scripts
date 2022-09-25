using UnityEngine;
public class PlayerLadderClimbState : PlayerBaseState
{
	float movement;
	float ladderEnterTime;
	float ladderCooldownTime = 1f;
	public override void EnterState(Player player)
	{
		player.transform.position = new Vector2 (player.ladderPosX, player.transform.position.y);
		player.animator.SetBool("IsClimbing", true);
		player.rb.gravityScale = 0;
		ladderEnterTime = Time.time;
	}
	public override void UpdateState(Player player)
	{
		// Stop slow controller movement
		if (Mathf.Abs(player.movement.y) > 0.5f)
		{
			movement = player.movement.y;
		}
		else
		{
			movement = 0;
		}
		// Stop climb animation when player stops
		if (Mathf.Abs(player.movement.y) < 0.5f)
		{
			player.animator.speed = 0;
		}
		else
		{
			player.animator.speed = 1;
		}
		// Player jumps
		if (player.jumpInput == 1)
		{
			player.rb.velocity = Vector2.zero;
			player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);

			ExitState(player, player.midairState);
		}
		// Player drops
		if (player.climbInput == 1 && Time.time > ladderEnterTime + ladderCooldownTime)
		{
			ExitState(player, player.midairState);
		}
		// Player reaches top of ladder
		if (player.transform.position.y > player.ladderMaxHeight)
		{
			player.transform.position = new Vector2(player.transform.position.x, player.ladderMaxHeight);
		}
		// Player reaches bottom of ladder
		if (player.transform.position.y < player.ladderMinHeight)
		{
			player.transform.position = new Vector2(player.transform.position.x, player.ladderMinHeight);
		}
	}
	public override void FixedUpdateState(Player player)
	{
		// Move the player
		player.rb.velocity = new Vector2(0, movement * player.ladderSpeed);
	}
	public override void ExitState(Player player, PlayerBaseState state)
	{
		player.animator.SetBool("IsClimbing", false);
		player.ladderExitTime = Time.time;
		player.animator.speed = 1;
		player.SwitchState(state);
	}
}