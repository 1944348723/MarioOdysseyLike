using UnityEngine;

public abstract class EntityBase: MonoBehaviour
{
    public bool IsGrounded => groundDetector.IsGrounded;
    public bool IsOnSlope => groundDetector.IsOnSlope;
    public float LastGoundedTime => groundDetector.LastGoundedTime;

    public EntityEvents entityEvents;

    protected GroundDetector groundDetector;
}

/// <summary>
/// CRTP(Curiously Recurring Template Pattern)
/// 子类继承时必须把自己作为泛型参数传递给父类
/// 该类负责提供移动相关功能以及驱动状态机
/// </summary>
public abstract class Entity<T>: EntityBase where T : Entity<T>
{
    public EntityStateManager<T> StateMachine { get; private set; }
    public Vector3 Velocity { get; set; }
    public Vector3 PlanarVelocity
    {
        get { return new Vector3(Velocity.x, 0, Velocity.z); }
        set { Velocity = new Vector3(value.x, Velocity.y, value.z); }
    }
    public Vector3 VerticalVelocity
    {
        get { return new Vector3(0, Velocity.y, 0); }
        set { Velocity = new Vector3(Velocity.x, value.y, Velocity.z); }
    }
    public Vector3 UnsizedPosition => transform.position;
    public float OriginalHeight { get; protected set; }
    // 倍率
    public float AccelerationMultiplier { get; set; } = 1f;
    public float DecelerationMultiplier { get; set; } = 1f;
    public float MaxSpeedMultiplier { get; set; } = 1f;
    public float TurningDragMultiplier { get; set; } = 1f;
    public float GravityMultiplier { get; set; } = 1f;
    
    protected CharacterController characterController; 

    protected virtual void Awake()
    {
        StateMachine = GetComponent<EntityStateManager<T>>();
        characterController = GetComponent<CharacterController>();
        groundDetector = GetComponent<GroundDetector>();
    }

    protected virtual void Start()
    {
        InitializeCharacterController();
        InitializeGroundDetector();
    }

    protected virtual void Update()
    {
        StateMachine.Step();
        Move();
        groundDetector.Tick(transform.position + characterController.center, Velocity.y <= 0);
    }

    public void Accelerate(Vector3 direction, float acceleration, float turningDrag, float maxSpeed)
    {
        Vector3 planarDir = Vector3.ProjectOnPlane(direction, Vector3.up);
        if (planarDir.sqrMagnitude < 1e-6f) return;
        planarDir.Normalize();

        // 当前速度在加速方向上的投影
        float forwardSpeed = Vector3.Dot(PlanarVelocity, planarDir);
        // 拆分当前平面速度：前向分量 + 侧向分量
        Vector3 forwardVelocity = planarDir * forwardSpeed;
        Vector3 turningVelocity = PlanarVelocity - forwardVelocity;

        // 沿输入方向加速
        float finalMaxSpeed = maxSpeed * MaxSpeedMultiplier;
        forwardSpeed += acceleration * AccelerationMultiplier * Time.deltaTime;
        forwardSpeed = Mathf.Clamp(forwardSpeed, -finalMaxSpeed, finalMaxSpeed);

        // 逐渐消除侧向速度
        turningVelocity = Vector3.MoveTowards(turningVelocity, Vector3.zero, turningDrag * TurningDragMultiplier * Time.deltaTime);

        // 合成并限速
        Vector3 newPlanarVelocity = forwardSpeed * planarDir + turningVelocity;
        if (newPlanarVelocity.sqrMagnitude > finalMaxSpeed * finalMaxSpeed) {
            newPlanarVelocity = newPlanarVelocity.normalized * finalMaxSpeed;
        }

        PlanarVelocity = newPlanarVelocity;
    }

    public void FaceToDirection(Vector3 direction, float degreesPerSecond)
    {
        if (direction == Vector3.zero) return;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion newRotation = Quaternion.RotateTowards(currentRotation, targetRotation, degreesPerSecond * Time.deltaTime);

        transform.rotation = newRotation;
    }

    public void Decelerate(float deceleration)
    {
        float deltaSpeed = deceleration * DecelerationMultiplier * Time.deltaTime;
        PlanarVelocity = Vector3.MoveTowards(PlanarVelocity, Vector3.zero, deltaSpeed);
    }

    public void SnapToGround(float speed)
    {
        // 防止影响到跳跃或者离地瞬间
        if (IsGrounded && Velocity.y <= 0)
        {
            VerticalVelocity = Vector3.down * speed;
        }
    }

    public void ResizeColliderHeight(float height)
    {
        float delta = height - characterController.height;
        characterController.height = height;
        characterController.center += 0.5f * delta * Vector3.up;
        groundDetector.Height = height;
    }

    private void InitializeCharacterController()
    {
        if (!characterController)
        {
            characterController = gameObject.AddComponent<CharacterController>();
        }

        characterController.skinWidth = characterController.radius * 0.1f;
        characterController.minMoveDistance = 0;
        OriginalHeight = characterController.height;
    }

    private void InitializeGroundDetector()
    {
        if (!groundDetector)
        {
            groundDetector = gameObject.AddComponent<GroundDetector>();
        }
        groundDetector.Init(
            characterController.height,
            characterController.radius,
            0.1f,
            characterController.slopeLimit,
            characterController.stepOffset
        );
        groundDetector.GroundEntered += OnGroundEntered;
        groundDetector.GroundExited += OnGroundExited;
    }

    private void Move()
    {
        if (characterController.enabled)
        {
            characterController.Move(Velocity * Time.deltaTime);
        } else
        {
            transform.position += Velocity * Time.deltaTime;
        }
    }

    protected void OnGroundEntered()
    {
        entityEvents.EnterGround?.Invoke();
    }

    protected void OnGroundExited()
    {
        // 在地面上时会有贴地处理，如果离地时有向下的贴地速度，就将其清除，
        VerticalVelocity = Vector3.Max(VerticalVelocity, Vector3.zero);
        entityEvents.ExitGround?.Invoke();
    }

}