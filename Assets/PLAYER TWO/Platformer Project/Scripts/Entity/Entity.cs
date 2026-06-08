using UnityEngine;

public abstract class EntityBase: MonoBehaviour
{
    public bool IsGrounded => groundDetector.IsGrounded;
    public bool IsOnSlope => groundDetector.IsOnSlope;
    public float LastGoundedTime => groundDetector.LastGoundedTime;
    public RaycastHit GroundHit => groundDetector.GroundHit;
    public GroundDetector groundDetector;
    public EntityModifierController ModifierController { get; protected set; }
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

    public EntityEvents entityEvents;
}

/// <summary>
/// CRTP(Curiously Recurring Template Pattern)
/// 子类继承时必须把自己作为泛型参数传递给父类
/// 该类负责提供移动相关功能以及驱动状态机
/// </summary>
public abstract class Entity<T>: EntityBase where T : Entity<T>
{
    public EntityStateManager<T> StateMachine { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Vector3 UnsizedPosition => transform.position;
    public float OriginalHeight { get; protected set; }

    protected virtual void Awake()
    {
        StateMachine = GetComponent<EntityStateManager<T>>();
        CharacterController = GetComponent<CharacterController>();
        groundDetector = GetComponent<GroundDetector>();
        ModifierController = gameObject.AddComponent<EntityModifierController>();
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
        groundDetector.Tick(transform.position + CharacterController.center, Velocity.y <= 0);
    }

    protected virtual void LateUpdate()
    {
        MoveWithPlatform();
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
        float finalMaxSpeed = maxSpeed * ModifierController.MaxSpeedMultiplier;
        forwardSpeed += acceleration * ModifierController.AccelerationMultiplier * Time.deltaTime;
        forwardSpeed = Mathf.Clamp(forwardSpeed, -finalMaxSpeed, finalMaxSpeed);

        // 逐渐消除侧向速度
        turningVelocity = Vector3.MoveTowards(turningVelocity, Vector3.zero, turningDrag * ModifierController.TurningDragMultiplier * Time.deltaTime);

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
        float deltaSpeed = deceleration * ModifierController.DecelerationMultiplier * Time.deltaTime;
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
        float delta = height - CharacterController.height;
        CharacterController.height = height;
        CharacterController.center += 0.5f * delta * Vector3.up;
        groundDetector.Height = height;
    }

    private void InitializeCharacterController()
    {
        if (!CharacterController)
        {
            CharacterController = gameObject.AddComponent<CharacterController>();
        }

        CharacterController.skinWidth = CharacterController.radius * 0.1f;
        CharacterController.minMoveDistance = 0;
        OriginalHeight = CharacterController.height;
    }

    private void InitializeGroundDetector()
    {
        if (!groundDetector)
        {
            groundDetector = gameObject.AddComponent<GroundDetector>();
        }
        groundDetector.Init(
            CharacterController.height,
            CharacterController.radius,
            0.1f,
            CharacterController.slopeLimit,
            CharacterController.stepOffset
        );
        groundDetector.GroundEntered += OnGroundEntered;
        groundDetector.GroundExited += OnGroundExited;
    }

    private void Move()
    {
        if (CharacterController.enabled)
        {
            CharacterController.Move(Velocity * Time.deltaTime);
        } else
        {
            transform.position += Velocity * Time.deltaTime;
        }
    }

    private void MoveWithPlatform()
    {
        if (IsGrounded && GroundHit.collider.TryGetComponent<MovingPlatform>(out var platform))
        {
            CharacterController.Move(platform.DeltaPosition);
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent<IEntityContact>(out var other))
        {
            other.OnEntityContact(this);
        }
    }
}