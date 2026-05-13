using UnityEngine;

// TODO: 当前在很陡的坡面上虽然判定为离地，但是不会往下掉，后续记得处理下
public class Player : Entity<Player>
{
    public PlayerInputSystem Input { get; protected set; }
    public PlayerStatsManager Stats { get; protected set; }
    public bool IsDead { get { return health.IsDead; }}
    public bool IsInWater { get; protected set; } = false;
    public int JumpCouter { get; protected set; } = 0;

    public PlayerEvents playerEvents;

    private DamageReceiver damageReceiver;
    private Health health;
    private float lastDashTime = 0f;

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
            VerticalVelocity += gravity * GravityMultiplier * Time.deltaTime * Vector3.down;
        }
    }

    public void SnapToGround() => SnapToGround(Stats.Current.snapSpeed);

    public bool TryJump()
    {
        if (!Input.HasBufferedJump() || !CanJump()) return false;
        Input.ConsumeBufferedJump();
        Jump(Stats.Current.maxJumpSpeed);
        return true;
    }

    public bool TryFall()
    {
        if (IsGrounded) return false;

        StateMachine.Change<FallPlayerState>();
        return true;
    }

    public bool TryCrouch()
    {
        // 检测式，不需要是这一帧按下的，这样在空中就可以一直按住下蹲键，落地就会直接蹲下
        if (!IsGrounded || !Input.IsCrouchPressed()) return false;
        
        StateMachine.Change<CrouchPlayerState>();
        return true;
    }
    public bool TryDash()
    {
        if (!Input.IsDashPressedThisFrame() || !CanDash()) return false;

        lastDashTime = Time.time;
        StateMachine.Change<DashPlayerState>();
        return true;
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
            --JumpCouter;
            StateMachine.Change<BackflipPlayerState>();
            playerEvents.Jumped?.Invoke();
            playerEvents.Backfliped?.Invoke();
        }
    }

    public bool CanDash()
    {
        bool canGroundDash = IsGrounded
            && Stats.Current.canGroundDash
            && Time.time - lastDashTime >= Stats.Current.dashCoolDown;
        bool canAirDash = !IsGrounded
            && Stats.Current.canAirDash
            && Time.time - lastDashTime >= Stats.Current.dashCoolDown;
        return canGroundDash || canAirDash;
    }


    private void ResetJumps() => JumpCouter = 0;
    
    private bool CanJump()
    {
        bool canCoyoteJump = JumpCouter == 0 && Time.time - LastGoundedTime < Stats.Current.coyoteJumpThreshold;
        bool canMultiJump = JumpCouter > 0 && JumpCouter < Stats.Current.allowedJumpTimes;
        
        return IsGrounded || canCoyoteJump || canMultiJump;
    }

    private void Jump(float speed)
    {
        if (speed <= 0)
        {
            Debug.LogError("[Player] Jump speed should be above 0");
            return;
        }

        ++JumpCouter;
        VerticalVelocity = Vector3.up * speed;
        StateMachine.Change<FallPlayerState>();
        playerEvents.Jumped?.Invoke();
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