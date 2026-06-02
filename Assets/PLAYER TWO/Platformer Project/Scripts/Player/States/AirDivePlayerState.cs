using UnityEngine;

public class AirDivePlayerState : PlayerState
{
    protected override void OnContact(Player player, ControllerColliderHit hit)
    {
    }

    protected override void OnEnter(Player player)
    {
        Debug.Log("AirDivePlayerState Entered");
        player.VerticalVelocity = Vector3.zero;
        player.PlanarVelocity = player.transform.forward * player.Stats.Current.airDiveForwardSpeed;
        player.playerEvents.AirDived?.Invoke();
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("AirDivePlayerState Exited");
    }

    protected override void OnStep(Player player)
    {
        if (player.TrySwim()) return;
        
        if (player.IsGrounded)
        {
            if (player.PlanarVelocity == Vector3.zero)
            {
                player.StateMachine.Change<IdlePlayerState>();
            } else
            {
                player.Decelerate(player.Stats.Current.airDiveFriction);
            }
        } else
        {
            player.Gravity();
        }
    }
}