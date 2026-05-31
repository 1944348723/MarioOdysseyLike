using UnityEngine;

public class PoleClimbingPlayerState : PlayerState
{
    private float distanceToPole = 0f;
    private readonly float heightOffsetToPolePeak = 0.3f;

    protected override void OnEnter(Player player)
    {
        Debug.Log("PoleClimbingPlayerState Entered");
        player.ResetJumps();
        FaceToPole(player);
        SnapToPole(player);
        distanceToPole = GetDistanceToPole(player);
        player.Velocity = Vector3.zero;
    }

    protected override void OnExit(Player player)
    {
        Debug.Log("PoleClimbingPlayerState Exited");
        player.RecordPoleExit();
    }

    protected override void OnStep(Player player)
    {
        if (player.IsGrounded)
        {
            player.StateMachine.Change<IdlePlayerState>();
            return;
        }
        if (TryJump(player)) return;

        HandleVerticalVelocity(player);
        HandlePlanarVelocity(player);
        FaceToPole(player);
        FixDistance(player);
        ClampHeight(player);
    }

    private void FaceToPole(Player player)
    {
        Pole pole = player.PoleDetector.CurrentPole;
        Vector3 dirToPole = (pole.transform.position - player.transform.position).normalized;
        dirToPole.y = 0;
        player.transform.rotation = Quaternion.LookRotation(dirToPole);
    }

    private void SnapToPole(Player player)
    {
        Vector3 polePoint = player.PoleDetector.CurrentPole.transform.position;
        polePoint.y = player.transform.position.y;
        Vector3 movement = polePoint - player.transform.position;
        player.CharacterController.Move(movement);
    }

    private void HandleVerticalVelocity(Player player)
    {
        Vector3 inputDirection = GameInputSystem.Instance.GetMovementDirection();
        if (inputDirection.z > 0)
        {
            player.VerticalVelocity = Vector3.up * player.Stats.Current.poleClimbUpSpeed;
        } else if (inputDirection.z < 0)
        {
            player.VerticalVelocity = Vector3.down * player.Stats.Current.poleClimbDownSpeed;
        } else
        {
            player.VerticalVelocity = Vector3.zero;
        }
    }

    private void HandlePlanarVelocity(Player player)
    {
        Vector3 inputDirection = GameInputSystem.Instance.GetMovementDirection();
        if (inputDirection.x == 0) {
            player.PlanarVelocity = Vector3.zero;
            return;
        }

        player.PlanarVelocity = player.Stats.Current.poleClimbRatationSpeed
            * (inputDirection.x > 0 ? 1 : -1) * player.transform.right;
    }

    private float GetDistanceToPole(Player player)
    {
        Vector3 polePoint = player.PoleDetector.CurrentPole.transform.position;
        polePoint.y = player.transform.position.y;
        return Vector3.Magnitude(polePoint - player.transform.position);
    }

    private void FixDistance(Player player)
    {
        Vector3 polePoint = player.PoleDetector.CurrentPole.transform.position;
        polePoint.y = player.transform.position.y;
        Vector3 poleToPlayerDir = (player.transform.position - polePoint).normalized;
        player.transform.position = polePoint + poleToPlayerDir * distanceToPole;
    }

    private bool TryJump(Player player)
    {
        Vector3 polePoint = player.PoleDetector.CurrentPole.transform.position;
        polePoint.y = player.transform.position.y;
        Vector3 poleToPlayerDir = (player.transform.position - polePoint).normalized;

        return player.TryDirectionalJump(poleToPlayerDir,
            player.Stats.Current.poleClimbJumpVerticalSpeed,
            player.Stats.Current.poleClimbJumpPlanarSpeed
        );
    }

    private void ClampHeight(Player player)
    {
        float maxY = player.PoleDetector.CurrentPole.MaxHeight - heightOffsetToPolePeak;
        Debug.Log(maxY);
        Debug.Log(player.transform.position.y);
        if (player.transform.position.y >= maxY)
        {
            Vector3 currentPosition = player.transform.position;
            Vector3 targetPosition = new(currentPosition.x, maxY, currentPosition.z);
            player.transform.position = targetPosition;
        }
    }
}