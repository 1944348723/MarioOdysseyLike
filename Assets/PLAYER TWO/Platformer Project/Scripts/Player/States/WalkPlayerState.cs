using UnityEngine;

public class WalkPlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("WalkPlayerState Entered");
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("WalkPlayerState Exited");
    }

    protected override void OnStep(Player player)
    {
        player.Gravity();
        
        Vector3 inputDirection = player.Input.GetMoveDirectionBasedOnCamera();

        if (inputDirection == Vector3.zero)
        {
            player.Friction();
            if (player.PlanarVelocity.magnitude == 0)
            {
                player.StateMachine.Change<IdlePlayerState>();
            }
        } else
        {
            // 单位向量点乘得到两向量间夹角的余弦值
            float cos = Vector3.Dot(inputDirection, player.PlanarVelocity.normalized);

            if (cos >= player.Stats.Current.brakeThreshold)
            {
                player.Accelerate(inputDirection);
                player.FaceToDirectionSmoothly(player.PlanarVelocity);
            } else
            {
                player.StateMachine.Change<BrakePlayerState>();
            }
        }

    }
}