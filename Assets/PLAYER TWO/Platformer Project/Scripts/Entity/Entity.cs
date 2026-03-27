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

    public Vector3 PlanarVelocity
    {
        get { return new Vector3(Velocity.x, 0, Velocity.z); }
        set { Velocity = new Vector3(value.x, Velocity.y, value.y); }
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
    }
}