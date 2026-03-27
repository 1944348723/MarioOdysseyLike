using UnityEngine;

// 继承自MonoBehaviour，为了让所有Entity能够挂载、使用Unity声明周期函数
public abstract class EntityBase: MonoBehaviour
{

}

// CRTP(Curiously Recurring Template Pattern)
// 子类继承时必须把自己作为泛型参数传递给父类
public abstract class Entity<T>: EntityBase where T : Entity<T>
{
    protected EntityStateManager<T> stateMachine;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<EntityStateManager<T>>();
    }

    protected virtual void Update()
    {
        stateMachine.Step();
    }
}