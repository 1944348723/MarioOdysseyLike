
using UnityEngine;

public class CrawlPlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        player.ResizeColliderHeight(player.Stats.Current.crouchHeight);
    }

    protected override void OnExit(Player player) {
        player.ResizeColliderHeight(player.OriginalHeight);
    }

    protected override void OnStep(Player player) {
        player.Gravity();
        player.SnapToGround();
        player.HandleJump();
        player.Fall();

        Vector3 inputDirection = player.Input.GetMoveDirectionBasedOnCamera();

        if (!player.Input.IsCrouchAndCrawlPressed() && player.CanStandUp())
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