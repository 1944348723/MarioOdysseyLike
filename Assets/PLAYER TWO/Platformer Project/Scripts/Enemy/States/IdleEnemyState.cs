using UnityEngine;

public class IdleEnemyState : EnemyState
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
    }

    protected override void OnContact(Enemy entity, ControllerColliderHit hit)
    {
        
    }
}