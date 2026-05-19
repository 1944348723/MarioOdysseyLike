using UnityEngine;

public class SwimPlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("SwimPlayerState Entered");
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("SwimPlayerState Exited");
    }

    protected override void OnStep(Player player)
    {
        if (TryJump(player)) return;

        HandlePlanarVerlocity(player);
        HandleVerticalVelocity(player);
    }

    private bool TryJump(Player player)
    {
        if (player.transform.position.y >= player.PlayerWaterDetector.CurrentWater.SurfaceY
            && player.Input.HasBufferedJump()
        )
        {
            player.Jump(player.Stats.Current.swimJumpSpeed);
            return true;
        }
        return false;
    }

    private void HandlePlanarVerlocity(Player player)
    {
        Vector3 inputDirection = player.Input.GetMoveDirectionBasedOnCamera();
        if (inputDirection != Vector3.zero)
        {
            player.Accelerate(
                inputDirection,
                player.Stats.Current.swimAcceleration,
                player.Stats.Current.swimTurningDrag,
                player.Stats.Current.swimMaxSpeed);
            player.FaceToDirectionSmoothly(player.PlanarVelocity);
        } else
        {
            player.Decelerate(player.Stats.Current.swimDeceleration);
        }
    }

    private void HandleVerticalVelocity(Player player)
    {
        // 下潜
        if (player.Input.IsDivePressed())
        {
            player.VerticalVelocity += player.Stats.Current.swimDiveAcceleration * Time.deltaTime * Vector3.down;
            if (player.VerticalVelocity.y < player.Stats.Current.swimDiveMaxSpeed)
            {
                player.VerticalVelocity = Vector3.down * player.Stats.Current.swimDiveMaxSpeed;
            }
            if (player.IsGrounded)
            {
                player.VerticalVelocity = Vector3.zero;
            }
        } else  // 上浮
        {
            if (player.IsGrounded && player.VerticalVelocity.y < 0)
            {
                player.VerticalVelocity = Vector3.zero;
            }
            if (player.transform.position.y < player.PlayerWaterDetector.CurrentWater.SurfaceY)
            {
                player.VerticalVelocity += player.Stats.Current.swimUpwardAcceleration * Time.deltaTime * Vector3.up;
                if (player.VerticalVelocity.y > player.Stats.Current.swimUpwardMaxSpeed)
                {
                    player.VerticalVelocity = Vector3.up * player.Stats.Current.swimUpwardMaxSpeed;
                }
            } else
            {
                // 在水面就只是飘着
                player.VerticalVelocity = Vector3.zero;
            }
        }
    }
}