using UnityEngine;

// 继承自MonoBehaviour，为了让所有Entity能够挂载、使用Unity声明周期函数
public abstract class EntityBase: MonoBehaviour
{

}

// CRTP(Curiously Recurring Template Pattern)
// 子类继承时必须把自己作为泛型参数传递给父类
public abstract class Entity<T>: EntityBase where T : Entity<T>
{
    public EntityStateManager<T> StateMachine { get; private set; }
    public Vector3 Velocity { get; set; }
    // 倍率
    public float AccelerationMultiplier { get; set; } = 1f;
    public float DecelerationMultiplier { get; set; } = 1f;
    public float MaxSpeedMultiplier { get; set; } = 1f;
    public float TurningGragMultiplier { get; set; } = 1f;
    public float GravityMultiplier { get; set; } = 1f;

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

    protected virtual void Awake()
    {
        StateMachine = GetComponent<EntityStateManager<T>>();
    }

    protected virtual void Update()
    {
        StateMachine.Step();
        Debug.Log(Velocity);
        transform.position += Velocity * Time.deltaTime;
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
        forwardSpeed += acceleration * Time.deltaTime;
        forwardSpeed = Mathf.Clamp(forwardSpeed, -maxSpeed, maxSpeed);

        // 逐渐消除侧向速度
        turningVelocity = Vector3.MoveTowards(turningVelocity, Vector3.zero, turningDrag * Time.deltaTime);

        // 合成并限速
        Vector3 newPlanarVelocity = forwardSpeed * planarDir + turningVelocity;
        if (newPlanarVelocity.sqrMagnitude > maxSpeed * maxSpeed) {
            newPlanarVelocity = newPlanarVelocity.normalized * maxSpeed;
        }

        PlanarVelocity = newPlanarVelocity;
    }
}