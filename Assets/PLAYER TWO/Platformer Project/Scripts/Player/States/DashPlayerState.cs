using UnityEngine;

public class DashPlayerState : PlayerState
{
    private float timer;

    protected override void OnContact(Player player, ControllerColliderHit hit)
    {
        player.PushRigidBody(hit);
    }

    protected override void OnEnter(Player player)
    {
        Debug.Log("DashPlayerState Entered");
        player.PlanarVelocity = player.transform.forward * player.Stats.Current.dashSpeed;
        player.VerticalVelocity = Vector3.zero;
        timer = 0;
        player.playerEvents.DashStarted?.Invoke();
    }

    protected override void OnExit(Player player) {
        Debug.Log("DashPlayerState Exited");
        player.PlanarVelocity = Vector3.ClampMagnitude(player.PlanarVelocity, player.Stats.Current.maxSpeed);
        player.playerEvents.DashEnded?.Invoke();
    }

    protected override void OnStep(Player player) {
        timer += Time.deltaTime;
        if (timer > player.Stats.Current.dashDuration)
        {
            timer = 0;
            if (player.IsGrounded)
            {
                player.StateMachine.Change<WalkPlayerState>();
            } else
            {
                player.StateMachine.Change<FallPlayerState>();
            }
        }
    }
}