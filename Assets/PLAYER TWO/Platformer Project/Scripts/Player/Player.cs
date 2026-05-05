using UnityEngine;

// TODO: 当前在很陡的坡面上虽然判定为离地，但是不会往下掉，后续记得处理下
public class Player : Entity<Player>
{
    public PlayerInputSystem Input { get; protected set; }
    public PlayerStatsManager Stats { get; protected set; }
    public bool IsDead { get { return health.IsDead; }}
    public bool IsInWater { get; protected set; } = false;

    public PlayerEvents playerEvents;

    private DamageReceiver damageReceiver;
    private Health health;
    private int jumpCouter = 0;

    protected override void Awake()
    {
        base.Awake();
        Input = GetComponent<PlayerInputSystem>();
        Stats = GetComponent<PlayerStatsManager>();
        damageReceiver = GetComponent<DamageReceiver>();
        health = GetComponent<Health>();

        entityEvents.EnterGround.AddListener(ResetJumps);
        damageReceiver.Damaged += OnDamaged;
    }

    public void Accelerate(Vector3 direction)
    {
        base.Accelerate(direction.normalized, Stats.Current.acceleration, Stats.Current.turningDrag, Stats.Current.maxSpeed);
    }

    public void AccelerateToInputDirection()
    {
        Vector3 direction = Input.GetMoveDirectionBasedOnCamera();
        Accelerate(direction);
    }

    public void FaceToDirectionSmoothly(Vector3 direction)
    {
        base.FaceToDirection(direction, Stats.Current.rotationSpeed);
    }

    // 主动减速
    public void Decelerate()
    {
        base.Decelerate(Stats.Current.deceleration);
    }

    // 摩擦减速
    public void Friction()
    {
        if (IsOnSlope)
        {
            base.Decelerate(Stats.Current.slopeFriction);
        } else
        {
            base.Decelerate(Stats.Current.friction);
        }
    }

    public void Gravity()
    {
        if (!IsGrounded && Velocity.y > -Stats.Current.maxFallingSpeed)
        {
            float speed = Velocity.y;
            // 上升时用正常重力，下落时用下落重力
            float gravity = speed > 0 ? Stats.Current.gravity : Stats.Current.fallGravity;
            speed -= gravity * GravityMultiplier * Time.deltaTime;
            speed = Mathf.Max(speed, -Stats.Current.maxFallingSpeed);
            VerticalVelocity = new Vector3(0, speed, 0);
        }
    }

    public void Gravity(float gravity)
    {
        if (gravity <= 0) return;

        if (!IsGrounded)
        {
            Debug.Log("Gravity");
            VerticalVelocity += gravity * GravityMultiplier * Time.deltaTime * Vector3.down;
        }
    }

    public void SnapToGround() => SnapToGround(Stats.Current.snapSpeed);

    public void HandleJump()
    {
        // 在地面上可以跳；还没有跳过但是离地了，如果在土狼跳允许时间内可以跳；已经跳过了但是在允许跳跃次数范围内可以再跳
        if (CanJump() && Input.HasBufferedJump())
        {
            Input.ConsumeBufferedJump();
            Jump(Stats.Current.maxJumpSpeed);
        }

        HandleJumpCut();
    }

    public void Fall()
    {
        if (!IsGrounded)
        {
            StateMachine.Change<FallPlayerState>();
        }
    }

    public bool CanStandUp()
    {
        return !Physics.SphereCast(
            transform.position + characterController.center,
            characterController.radius,
            Vector3.up,
            out _,
            characterController.height / 2);
    }

    public void BackFlip()
    {
        if (Stats.Current.canBackFlip)
        {
            VerticalVelocity = Vector3.up * Stats.Current.backflipUpwardSpeed;
            PlanarVelocity = -transform.forward * Stats.Current.backflipBackwardSpeed;
            --jumpCouter;
            StateMachine.Change<BackflipPlayerState>();
            playerEvents.Jumped?.Invoke();
            playerEvents.Backfliped?.Invoke();
        }
    }

    private void ResetJumps() => jumpCouter = 0;
    
    private bool CanJump()
    {
        bool canCoyoteJump = jumpCouter == 0 && Time.time - LastGoundedTime < Stats.Current.coyoteJumpThreshold;
        bool canMultiJump = jumpCouter > 0 && jumpCouter < Stats.Current.allowedJumpTimes;
        
        return IsGrounded || canCoyoteJump || canMultiJump;
    }

    private void Jump(float speed)
    {
        if (speed <= 0)
        {
            Debug.LogError("[Player] Jump speed should be above 0");
            return;
        }

        ++jumpCouter;
        VerticalVelocity = Vector3.up * speed;
        StateMachine.Change<FallPlayerState>();
        playerEvents.Jumped?.Invoke();
    }

    private void HandleJumpCut()
    {
        // 跳跃上升中松开跳跃键会跳的比较低
        if (Input.IsJumpReleasedThisFrame() && (jumpCouter > 0) && (Velocity.y > Stats.Current.minJumpSpeed))
        {
            VerticalVelocity = Vector3.up * Stats.Current.minJumpSpeed;
        }
    }

    private void OnDamaged(DamageInfo info)
    {
        Vector3 dirToDamageSource = info.sourcePosition - transform.position;
        Vector3 planarDirToDamageSource = new(dirToDamageSource.x, 0, dirToDamageSource.z);
        planarDirToDamageSource.Normalize();

        transform.LookAt(transform.position + planarDirToDamageSource);
        PlanarVelocity = -planarDirToDamageSource * Stats.Current.hurtBackwardSpeed;
        if (!IsInWater)
        {
            VerticalVelocity = Vector3.up * Stats.Current.hurtUpwardSpeed;
            StateMachine.Change<HurtPlayerState>();
        }

        playerEvents.Hurt?.Invoke();
    }
}