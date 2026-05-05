using UnityEngine;

public class BackflipPlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        if (player.Stats.Current.lockMovementDuringBackflip)
        {
            player.Input.MoveLocked = true;
        }
    }

    protected override void OnExit(Player player) {
        if (player.Stats.Current.lockMovementDuringBackflip)
        {
            player.Input.MoveLocked = false;
        }
    }

    protected override void OnStep(Player player)
    {
        player.Gravity(player.Stats.Current.backflipGravity);
        Vector3 inputDirection = player.Input.GetMoveDirectionBasedOnCamera();
        if (inputDirection != Vector3.zero)
        {
            player.Accelerate(
                inputDirection,
                player.Stats.Current.backflipAirAcceleration,
                player.Stats.Current.backflipAirTurningSpeed,
                player.Stats.Current.backflipAirMaxSpeed
            );
            player.FaceToDirectionSmoothly(player.PlanarVelocity);
        }

        if (player.IsGrounded)
        {
            player.PlanarVelocity = Vector3.zero;
            player.StateMachine.Change<IdlePlayerState>();
        }
    }
}