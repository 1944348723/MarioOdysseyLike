
using UnityEngine;

public class CrawlPlayerState : PlayerState
{
    protected override void OnContact(Player player, ControllerColliderHit hit)
    {
        player.PushRigidBody(hit);
    }

    protected override void OnEnter(Player player)
    {
        player.ResizeColliderHeight(player.Stats.Current.crouchHeight);
    }

    protected override void OnExit(Player player) {
        player.ResizeColliderHeight(player.OriginalHeight);
    }

    protected override void OnStep(Player player) {
        if (player.TryJump()) return;
        if (player.TryFall()) return;

        player.SnapToGround();

        Vector3 inputDirection = GameInputSystem.Instance.GetMoveDirectionBasedOnCamera();

        if (!GameInputSystem.Instance.IsCrouchAndCrawlPressed() && player.CanStandUp())
        {
            if (player.PlanarVelocity != Vector3.zero)
            {
                player.StateMachine.Change<WalkPlayerState>();
            } else
            {
                player.StateMachine.Change<IdlePlayerState>();
            }
        } else
        {
            if (inputDirection != Vector3.zero)
            {
                player.Accelerate(
                    inputDirection,
                    player.Stats.Current.crawlAcceleration,
                    player.Stats.Current.crawlTurningSpeed,
                    player.Stats.Current.crawlMaxSpeed
                );
                player.FaceToDirectionSmoothly(player.PlanarVelocity);
            } else
            {
                player.Decelerate(player.Stats.Current.crawlFriction);
            }
        }
    }
}