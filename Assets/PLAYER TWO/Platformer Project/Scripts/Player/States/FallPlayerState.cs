
using UnityEngine;

public class FallPlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("FallPlayerState Entered");
    }

    protected override void OnStep(Player player)
    {
        player.Gravity();
        player.HandleJump();
        player.FaceToDirectionSmoothly(player.PlanarVelocity);

        if (player.IsGrounded)
        {
            if (player.PlanarVelocity.sqrMagnitude > 0)
            {
                player.StateMachine.Change<WalkPlayerState>();
            } else
            {
                player.StateMachine.Change<IdlePlayerState>();
            }
        }
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("FallPlayerState Exited");
    }
}