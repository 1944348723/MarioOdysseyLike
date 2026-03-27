using UnityEngine;

public class IdlePlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("IdlePlayerState Exited");
    }

    protected override void OnStep(Player player)
    {
        Vector3 inputDirection = player.Input.GetMovementDirection();
        if (inputDirection.sqrMagnitude > 0 || player.PlanarVelocity.sqrMagnitude > 0)
        {
            player.StateMachine.Change<WalkPlayerState>();
        }
    }
}