using UnityEngine;

public class IdlePlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("IdlePlayerState Entered");
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("IdlePlayerState Exited");
    }

    protected override void OnStep(Player player)
    {
        player.Gravity();
        player.HandleJump();

        Vector3 inputDirection = player.Input.GetMovementDirection();
        if (inputDirection.sqrMagnitude > 0 || player.PlanarVelocity.sqrMagnitude > 0)
        {
            player.StateMachine.Change<WalkPlayerState>();
        }
    }
}