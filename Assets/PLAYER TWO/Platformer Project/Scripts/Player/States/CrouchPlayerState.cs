
using UnityEngine;

public class CrouchPlayerState : PlayerState
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
        player.Fall();
        player.Decelerate(player.Stats.Current.crouchFriction);

        // 站起来
        if (!player.Input.IsCrouchAndCrawlPressed() && player.CanStandUp())
        {
            if (player.VerticalVelocity != Vector3.zero)
            {
                player.StateMachine.Change<WalkPlayerState>();
            } else
            {
                player.StateMachine.Change<IdlePlayerState>();
            }
            return;
        }

        // 有输入方向且速度为0则爬行
        Vector3 inputDirection = player.Input.GetMoveDirectionBasedOnCamera();
        if (inputDirection != Vector3.zero && player.PlanarVelocity == Vector3.zero)
        {
            // player.StateMachine.Change<CrawlPlayerState>();
        }
    }
}