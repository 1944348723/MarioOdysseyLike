// 使用泛型让状态绑定宿主实体类型，在使用时不会类型错配
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EntityState<T> where T : Entity<T>
{
    public float TimeSinceEntered { get; private set; }

    public UnityEvent Entered;
    public UnityEvent Exited;

    public static EntityState<T> CreateFromString(string typeName)
    {
        Type type = System.Type.GetType(typeName);
        if (type == null || !typeof(EntityState<T>).IsAssignableFrom(type))
        {
            Debug.LogError($"Invalid state type: {typeName}");
            return null;
        }

        return System.Activator.CreateInstance(type) as EntityState<T>;
    }

    public static List<EntityState<T>> CreateListFromStringArray(string[] array)
    {
        if (array == null || array.Length == 0) {
            return new List<EntityState<T>>();
        }

        List<EntityState<T>> states = new();
        foreach (string typeName in array) {
            EntityState<T> state = CreateFromString(typeName);
            if (state != null) {
                states.Add(state);
            }
        }

        return states;
    }

    public void Enter(T entity)
    {
        TimeSinceEntered = 0;
        Entered?.Invoke();
        OnEnter(entity);
    }

    public void Step(T entity)
    {
        TimeSinceEntered += Time.deltaTime;
        OnStep(entity);
    }

    public void Exit(T entity)
    {
        Exited?.Invoke();
        OnExit(entity);
    }

    protected abstract void OnEnter(T entity);
    protected abstract void OnStep(T entity);
    protected abstract void OnExit(T entity);
}