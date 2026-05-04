
using UnityEngine;

public class HurtPlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("HurtPlayerState Entered");
    }

    protected override void OnExit(Player player) {
        Debug.Log("HurtPlayerState Exited");
    }

    protected override void OnStep(Player player) {
        player.Gravity();
        
        if (player.IsGrounded && player.Velocity.y <= 0)
        {
            if (player.IsDead)
            {
                player.StateMachine.Change<DiePlayerState>();
            } else
            {
                player.StateMachine.Change<IdlePlayerState>();
            }
        }
    }
}