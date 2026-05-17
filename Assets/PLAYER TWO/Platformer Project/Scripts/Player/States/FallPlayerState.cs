
using UnityEngine;

public class FallPlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("FallPlayerState Entered");
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("FallPlayerState Exited");
    }

    protected override void OnStep(Player player)
    {
        HandleJumpCut(player);
        if (player.TryJump()) return;
        if (player.TryDash()) return;
        if (player.TryStomp()) return;
        if (player.TrySpin()) return;
        if (player.TryAirDive()) return;

        player.Gravity();
        player.AccelerateToInputDirection();
        player.FaceToDirectionSmoothly(player.PlanarVelocity);

        if (player.IsGrounded && player.VerticalVelocity.y <= 0)
        {
            if (player.PlanarVelocity != Vector3.zero)
            {
                player.StateMachine.Change<WalkPlayerState>();
            } else
            {
                player.StateMachine.Change<IdlePlayerState>();
            }
        }
    }

    
    private void HandleJumpCut(Player player)
    {
        // 跳跃上升中松开跳跃键会跳的比较低
        if (player.Input.IsJumpReleasedThisFrame()
            && player.JumpCouter > 0
            && player.Velocity.y > player.Stats.Current.minJumpSpeed)
        {
            player.VerticalVelocity = Vector3.up * player.Stats.Current.minJumpSpeed;
        }
    }
}