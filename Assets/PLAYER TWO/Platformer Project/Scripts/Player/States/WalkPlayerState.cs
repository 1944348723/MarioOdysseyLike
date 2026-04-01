using UnityEngine;

public class WalkPlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("WalkPlayerState Entered");
    }

    protected override void OnExit(Player player)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnStep(Player player)
    {
        Vector3 inputDirection = player.Input.GetMoveDirectionBasedOnCamera();
        if (inputDirection.sqrMagnitude < 1e-6) return;

        // 单位向量点乘得到两向量间夹角的余弦值
        float cos = Vector3.Dot(inputDirection, player.PlanarVelocity.normalized);
        if (cos >= player.Stats.Current.brakeThreshold)
        {
            player.Accelerate(inputDirection);
            player.FaceToDirectionSmoothly(player.PlanarVelocity);
        }
    }
}