
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
        player.Gravity();
        player.Decelerate();
        player.HandleJump();
        if (player.PlanarVelocity.sqrMagnitude == 0) {
            player.StateMachine.Change<IdlePlayerState>();
        }
    }
}