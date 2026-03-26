using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStateManager: MonoBehaviour
{
    
}


public abstract class EntityStateManager<T> : EntityStateManager where T : Entity<T>
{
    protected List<EntityState<T>> statesList;
    protected Dictionary<Type, EntityState<T>> statesMap = new();
    protected EntityState<T> currentState;
    protected T entity;

    protected abstract List<EntityState<T>> GetStatesList();

    protected void Awake()
    {
        entity = GetComponent<T>();
    }

    protected void Start()
    {
        InitializeStates();
    }

    public void Step()
    {
        if (currentState != null && Time.timeScale > 0)
        {
            currentState.Step(entity);
        }
    }

    /// <summary>
    /// 根据Inspector中的编辑创建对应的状态
    /// 整个状态到创建为止的流程：
    /// 1. 通过编辑器工具通过反射将状态类的名字显示在Inspector中供选择，避免用户手动输入字符串出错
    /// 2. 用户编辑后，启动时状态类的名字以字符串数组形式存入此类的子类中
    /// 3. 根据状态名字符串数组再用反射创建对应状态
    /// 4. 将创建的状态添加到字典中，便于查找
    /// </summary>
    protected void InitializeStates()
    {
        this.statesList = GetStatesList();

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
            currentState = statesList[0];
        }
    }

}