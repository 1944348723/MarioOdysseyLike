using UnityEngine;

public class IdlePlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("IdlePlayerState Entered");
        player.PlanarVelocity = Vector3.zero;
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("IdlePlayerState Exited");
    }

    protected override void OnStep(Player player)
    {
        if (player.TryJump()) return;
        if (player.TryFall()) return;
        if (player.TryCrouch()) return;

        player.SnapToGround();

        Vector3 inputDirection = player.Input.GetMovementDirection();
        if (inputDirection != Vector3.zero)
        {
            player.StateMachine.Change<WalkPlayerState>();
        }
    }
}