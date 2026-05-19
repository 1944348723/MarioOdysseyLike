
using UnityEngine;

public class GlidePlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("GlidePlayerState Entered");
        player.VerticalVelocity = Vector3.zero;
        player.playerEvents.GlideStarted?.Invoke();
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("GlidePlayerState Exited");
        player.playerEvents.GlideEnded?.Invoke();
    }

    protected override void OnStep(Player player)
    {
        if (!player.Input.IsGlidePressed())
        {
            player.StateMachine.Change<FallPlayerState>();
            return;
        }
        if (player.IsGrounded)
        {
            player.StateMachine.Change<WalkPlayerState>();
        }
        
        // 平面速度
        Vector3 inputDirection = player.Input.GetMoveDirectionBasedOnCamera();
        player.Accelerate(
            inputDirection,
            player.Stats.Current.airAcceleration,
            player.Stats.Current.glideTurningDrag,
            player.Stats.Current.maxSpeed
        );
        player.FaceToDirectionSmoothly(player.PlanarVelocity);

        // 垂直速度
        player.VerticalVelocity += player.Stats.Current.glideGravity * Time.deltaTime * Vector3.down;
        if (player.VerticalVelocity.y < player.Stats.Current.glideMaxFallingSpeed)
        {
            player.VerticalVelocity = Vector3.down * player.Stats.Current.glideMaxFallingSpeed;
        }
    }
}