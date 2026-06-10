using UnityEngine;

public class PatrolEnemyState : EnemyState
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

        UpdateDestination(enemy);

        Vector3 destination = enemy.Route.Current;
        Vector3 direction = (destination - enemy.transform.position).normalized;
        enemy.Accelerate(
            direction,
            enemy.Stats.Current.patrolAcceleration,
            enemy.Stats.Current.turningDrag,
            enemy.Stats.Current.patrolMaxSpeed
        );
        enemy.FaceToDirection(direction, enemy.Stats.Current.rotationSpeed);
    }

    protected override void OnContact(Enemy entity, ControllerColliderHit hit)
    {
        
    }

    private void UpdateDestination(Enemy enemy)
    {
        float distance = (enemy.Route.Current - enemy.transform.position).magnitude;
        if (distance < enemy.Stats.Current.checkDistance)
        {
            enemy.Route.Advance();
        }
    }
}