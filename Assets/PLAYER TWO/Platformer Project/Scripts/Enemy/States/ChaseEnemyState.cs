using UnityEngine;

public class ChaseEnemyState : EnemyState
{
    protected override void OnEnter(Enemy enemy)
    {
    }

    protected override void OnExit(Enemy enemy)
    {
        
    }

    protected override void OnStep(Enemy enemy)
    {
        enemy.Gravity();
        enemy.SnapToGround();
        if (!enemy.Vision.CanSeeTarget)
        {
            enemy.StateMachine.Change<PatrolEnemyState>();
            return;
        }

        Vector3 directionToTarget = (enemy.Vision.Target.position - enemy.transform.position).normalized;
        enemy.Accelerate(
            directionToTarget,
            enemy.Stats.Current.ChaseAcceleration,
            enemy.Stats.Current.turningDrag,
            enemy.Stats.Current.ChaseMaxSpeed
        );
        Vector3 planarDirectionToTarget = new(directionToTarget.x, 0, directionToTarget.z);
        enemy.FaceToDirection(planarDirectionToTarget, enemy.Stats.Current.rotationSpeed);
    }

    protected override void OnContact(Enemy entity, ControllerColliderHit hit)
    {
        
    }
}