using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStateManager: MonoBehaviour
{
    public EntityStateManagerEvents events;
}


public abstract class EntityStateManager<T> : EntityStateManager where T : Entity<T>
{
    protected List<EntityState<T>> statesList;
    protected Dictionary<Type, EntityState<T>> statesMap = new();
    protected T entity;

    public EntityState<T> CurrentState { get; private set; }
    public EntityState<T> LastState { get; private set; }
    public int CurrentStateIndex => statesList.IndexOf(CurrentState);
    public int LastStateIndex => statesList.IndexOf(LastState);

    protected abstract List<EntityState<T>> GetStatesList();

    protected virtual void Awake()
    {
        entity = GetComponent<T>();
    }

    protected virtual void Start()
    {
        InitializeStates();
    }

    public void Step()
    {
        if (CurrentState != null && Time.timeScale > 0)
        {
            CurrentState.Step(entity);
        }
    }


    /// <summary>
    /// 转换入口，告知要转换到什么状态
    /// </summary>
    public void Change<TState>() where TState : EntityState<T>
    {
        Type type = typeof(TState);
        
        if (this.statesMap.ContainsKey(type))
        {
            Change(statesMap[type]);
        }
    }

    /// <summary>
    /// 转换至具体状态实例
    /// </summary>
    private void Change(EntityState<T> to)
    {
        if (to == null || Time.timeScale == 0) return;

        if (CurrentState != null)
        {
            CurrentState.Exit(entity);
            events.Exited?.Invoke(CurrentState.GetType());
            LastState = CurrentState;
        }

        CurrentState = to;
        CurrentState.Enter(entity);
        events.Entered?.Invoke(CurrentState.GetType());
        events.Changed?.Invoke();
    }

    /// <summary>
    /// 根据Inspector中的编辑创建对应的状态
    /// 整个状态到创建为止的流程：
    /// 1. 通过编辑器工具通过反射将状态类的名字显示在Inspector中供选择，避免用户手动输入字符串出错
    /// 2. 用户编辑后，启动时状态类的名字以字符串数组形式存入此类的子类中
    /// 3. 根据状态名字符串数组再用反射创建对应状态
    /// 4. 将创建的状态添加到字典中，便于查找
    /// </summary>
    private void InitializeStates()
    {
        // 状态实例列表
        this.statesList = GetStatesList();

        // 状态实例根据类型存入哈希表中
        foreach (EntityState<T> state in this.statesList)
        {
            Type type = state.GetType();

            if (!statesMap.ContainsKey(type))
            {
                statesMap.Add(type, state);
            }
        }

        if (statesList.Count > 0)
        {
            CurrentState = statesList[0];
        }
    }

}