using UnityEngine;

public class Player : Entity<Player>
{
    public PlayerInputSystem Input { get; protected set; }
    public PlayerStatsManager Stats { get; protected set; }
    public int JumpCouter { get; protected set; } = 0;
    public PlayerEvents Events;

    protected override void Awake()
    {
        base.Awake();
        Input = GetComponent<PlayerInputSystem>();
        Stats = GetComponent<PlayerStatsManager>();
    }

    public void Accelerate(Vector3 direction)
    {
        base.Accelerate(direction.normalized, Stats.Current.acceleration, Stats.Current.turningDrag, Stats.Current.maxSpeed);
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
        // TODO: 去除IsGrounded状态写死
        IsGrounded = false;
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

    public void HandleJump()
    {
        // TODO: 去除IsGrounded写死
        IsGrounded = true;
        bool canCoyoteJump = JumpCouter == 0 && Time.time - LastGoundedTime < Stats.Current.coyoteJumpThreshold;
        bool canMultiJump = JumpCouter > 0 && JumpCouter < Stats.Current.allowedJumpTimes;

        // 在地面上可以跳；还没有跳过但是离地了，如果在土狼跳允许时间内可以跳；已经跳过了但是在允许跳跃次数范围内可以再跳
        if (IsGrounded || canCoyoteJump || canMultiJump)
        {
            if (Input.HasBufferedJump())
            {
                Input.ConsumeBufferedJump();
                Jump(Stats.Current.maxJumpSpeed);
            }
        }

        // 跳跃上升中松开跳跃键会跳的比较低
        if (Input.IsJumpReleasedThisFrame() && (JumpCouter > 0) && (Velocity.y > Stats.Current.minJumpSpeed))
        {
            VerticalVelocity = Vector3.up * Stats.Current.minJumpSpeed;
        }
    }

    private void Jump(float speed)
    {
        if (speed <= 0)
        {
            Debug.LogWarning("[Player] Jump speed should be above 0");
            return;
        }

        ++JumpCouter;
        VerticalVelocity = Vector3.up * speed;
        StateMachine.Change<FallPlayerState>();
        Events.Jumped?.Invoke();
    }
}