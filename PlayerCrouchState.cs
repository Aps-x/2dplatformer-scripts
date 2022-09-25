using UnityEngine;
public class PlayerCrouchState : PlayerBaseState
{
	public override void EnterState(Player player)
	{
		player.rb.velocity = Vector2.zero;
		player.animator.SetBool("IsCrouching", true);
		player.headCollider.enabled = false;
		if (player.oneWayPlatformCollider != null)
		{
			player.StartCoroutine("DropDown");
		}
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
		// Player no longer crouching
		if (player.crouchInput == 0)
		{
			ExitState(player, player.idleState);
		}
		// Player leaves the ground
		if (!player.onGround)
		{
			ExitState(player, player.midairState);
		}
		// Stop sliding after being pushed
		if (!player.onPlatform && !player.onConveyorBelt && player.onGround)
		{
			player.rb.velocity *= new Vector2(0, 1);
		}
	}
	public override void FixedUpdateState(Player player)
	{

	}
	public override void ExitState(Player player, PlayerBaseState state)
	{
		player.animator.SetBool("IsCrouching", false);
		player.headCollider.enabled = true;
		player.SwitchState(state);
	}
}