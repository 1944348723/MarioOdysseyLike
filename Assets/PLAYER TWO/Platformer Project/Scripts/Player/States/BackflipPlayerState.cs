using UnityEngine;

public class BackflipPlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        player.VerticalVelocity = Vector3.up * player.Stats.Current.backflipUpwardSpeed;
        player.PlanarVelocity = -player.transform.forward * player.Stats.Current.backflipBackwardSpeed;
        if (player.Stats.Current.lockMovementDuringBackflip)
        {
            GameInputSystem.Instance.MoveLocked = true;
        }
        player.playerEvents.Jumped?.Invoke();
        player.playerEvents.Backfliped?.Invoke();
    }

    protected override void OnExit(Player player) {
        if (player.Stats.Current.lockMovementDuringBackflip)
        {
            GameInputSystem.Instance.MoveLocked = false;
        }
    }

    protected override void OnStep(Player player)
    {
        if (player.TryStomp()) return;
        if (player.TrySwim()) return;

        if (player.IsGrounded && player.VerticalVelocity.y <= 0)
        {
            player.PlanarVelocity = Vector3.zero;
            player.StateMachine.Change<IdlePlayerState>();
        }

        player.Gravity(player.Stats.Current.backflipGravity);
        Vector3 inputDirection = GameInputSystem.Instance.GetMoveDirectionBasedOnCamera();
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

    }
}