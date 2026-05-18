using UnityEngine;

public class SpinPlayerState : PlayerState
{
    private float timer = 0f;

    protected override void OnEnter(Player player)
    {
        Debug.Log("SpinPlayerState Entered");
        if (!player.IsGrounded)
        {
            player.VerticalVelocity = new(0, player.Stats.Current.airSpinUpwardSpeed, 0);
        }
        timer = 0f;
        player.playerEvents.SpinStarted?.Invoke();
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("SpinPlayerState Exited");
        player.playerEvents.SpinEnded?.Invoke();
    }

    protected override void OnStep(Player player)
    {
        if (timer >= player.Stats.Current.spinDuration)
        {
            if (!player.IsGrounded)
            {
                player.StateMachine.Change<FallPlayerState>();
            } else
            {
                if (player.PlanarVelocity == Vector3.zero)
                {
                    player.StateMachine.Change<IdlePlayerState>();
                } else
                {
                    player.StateMachine.Change<WalkPlayerState>();
                }
            }
            return;
        }

        timer += Time.deltaTime;
        player.Gravity();
        player.SnapToGround();
        player.AccelerateToInputDirection();
        if (player.Input.GetMoveDirectionBasedOnCamera() == Vector3.zero)
        {
            player.Friction();
        }
    }
}