using UnityEngine;

public class BrakePlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("BrakePlayerState Entered");
    }

    protected override void OnExit(Player player) {
        Debug.Log("BrakePlayerState Exited");
    }

    protected override void OnStep(Player player) {
        if (player.TryBackFlip()) return;
        if (player.TryFall()) return;

        Vector3 inputDirection = player.Input.GetMoveDirectionBasedOnCamera();
        float cos = Vector3.Dot(inputDirection, player.PlanarVelocity.normalized);

        if (inputDirection != Vector3.zero && cos < 0)
        {
            player.Gravity();
            player.SnapToGround();
            player.Decelerate();

            if (player.PlanarVelocity == Vector3.zero)
            {
                player.StateMachine.Change<IdlePlayerState>();
            }
        } else {
            if (player.PlanarVelocity == Vector3.zero) {
                player.StateMachine.Change<IdlePlayerState>();
            } else
            {
                player.StateMachine.Change<WalkPlayerState>();
            }
        }
    }
}