using UnityEngine;

public class DeathEnemyState : EnemyState
{
    protected override void OnEnter(Enemy enemy)
    {
        enemy.DisableHitBox();
        enemy.PlanarVelocity = Vector3.zero;
        DOVirtual.DelayedCall(1, () => enemy.gameObject.SetActive(false));
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