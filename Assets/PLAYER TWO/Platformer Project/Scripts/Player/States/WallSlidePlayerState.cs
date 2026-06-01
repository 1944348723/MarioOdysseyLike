using UnityEngine;

public class WallSlidePlayerState : PlayerState
{
    private Vector3 wallNormal;

    protected override void OnContact(Player player, ControllerColliderHit hit)
    {
    }

    protected override void OnEnter(Player player)
    {
        Debug.Log("WallSlidePlayerState Entered");
        player.ResetJumps();
        SnapToWall(player);
        player.transform.rotation = Quaternion.LookRotation(player.WallDetector.WallNormal);
        player.PlanarVelocity = Vector3.zero;
        player.VerticalVelocity = Vector3.down * player.Stats.Current.wallSlideSpeed;
        wallNormal = player.WallDetector.WallNormal.normalized;
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("WallSlidePlayerState Exited");
    }

    protected override void OnStep(Player player)
    {
        // 这样能从不接地的墙上掉下来
        player.WallDetector.Check(-wallNormal);

        if (player.IsGrounded)
        {
            player.StateMachine.Change<IdlePlayerState>();
            return;
        }

        if (!player.WallDetector.HasWall)
        {
            player.StateMachine.Change<FallPlayerState>();
            return;
        }

        Vector3 inputDirection = GameInputSystem.Instance.GetMoveDirectionBasedOnCamera();
        bool isInputDirectionAwayFromWall = AreDirectionsOnSameSide(inputDirection, wallNormal);
        if (inputDirection == Vector3.zero || isInputDirectionAwayFromWall)
        {
            player.StateMachine.Change<FallPlayerState>();
            return;
        }

        TryWallJump(player);
    }

    private bool AreDirectionsOnSameSide(Vector3 direction1, Vector3 direction2)
    {
        float cos = Vector3.Dot(direction1, direction2);
        return cos > 0;
    }

    private void SnapToWall(Player player)
    {
        Vector3 wallPoint = player.WallDetector.WallPoint;
        Vector3 movement = wallPoint - player.transform.position;
        movement.y = 0;
        player.CharacterController.Move(movement);
    }

    private bool TryWallJump(Player player)
    {
        return player.TryDirectionalJump(wallNormal,
            player.Stats.Current.wallJumpVerticalSpeed,
            player.Stats.Current.wallJumpPlanarSpeed
        );
    }
}