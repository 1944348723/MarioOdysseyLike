using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyStatsManager))]
[RequireComponent(typeof(DamageReceiver))]
public class Enemy : Entity<Enemy>
{
    [SerializeField] private GameObject hitBox;

    public EnemyStatsManager Stats { get; protected set; }

    private Health health;
    private DamageReceiver damageReceiver;

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Health>();
        Stats = GetComponent<EnemyStatsManager>();
        damageReceiver = GetComponent<DamageReceiver>();
    }

    protected override void Start()
    {
        base.Start();
        health.Died += OnDeath;
    }

    protected void OnDestroy()
    {
        health.Died -= OnDeath;
    }
    
    public void SnapToGround() => SnapToGround(Stats.Current.snapSpeed);

    public void Gravity()
    {
        if (!IsGrounded && Velocity.y > -Stats.Current.maxFallingSpeed)
        {
            float speed = Velocity.y;
            // 上升时用正常重力，下落时用下落重力
            float gravity = speed > 0 ? Stats.Current.gravity : Stats.Current.fallGravity;
            speed -= gravity * ModifierController.GravityMultiplier * Time.deltaTime;
            speed = Mathf.Max(speed, -Stats.Current.maxFallingSpeed);
            VerticalVelocity = new Vector3(0, speed, 0);
        }
    }

    public void DisableHitBox()
    {
        hitBox.SetActive(false);
    }

    private void OnDeath()
    {
        StateMachine.Change<DeathEnemyState>();
        DOVirtual.DelayedCall(1, () => gameObject.SetActive(false));
    }
}