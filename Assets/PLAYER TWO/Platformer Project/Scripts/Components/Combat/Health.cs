using System;
using UnityEngine;

public readonly struct HealthChangedInfo
{
    public readonly int OldHealth { get; }
    public readonly int NewHealth { get; }
    public readonly int Delta { get; }

    internal HealthChangedInfo(int oldHealth, int newHealth)
    {
        if (oldHealth < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(oldHealth));
        }
        if (newHealth < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newHealth));
        }
        this.OldHealth = oldHealth;
        this.NewHealth = newHealth;
        Delta = newHealth - oldHealth;
    }
}

public class Health : MonoBehaviour
{
    [SerializeField] private int max;
    [SerializeField] private int init;

    public int Current { get; private set; }
    public bool IsDead { get; private set; } = false;

    public event Action<HealthChangedInfo> Changed;
    public event Action Died;
    public event Action Revived;

    private void OnValidate()
    {
        if (max <= 0) max = 1;
        if (init <= 0 || init > max) init = max;
    }

    private void Start()
    {
        Current = init;
    }

    public bool Damage(int value)
    {
        if (value < 0)
        {
            Debug.LogWarning("Damage value can't be negative.", this);
            return false;
        }
        if (IsDead) return false;

        int prev = Current;
        Current -= value;
        if (Current < 0) {
            Current = 0;
            IsDead = true;
        }

        int change = Current - prev;
        // 先通知改变再通知死亡
        if (change != 0)
        {
            Changed?.Invoke(new HealthChangedInfo(prev, Current));
        }
        if (Current <= 0)
        {
            Died?.Invoke();
        }
        return true;
    }

    // 目前治疗值为负数直接抛异常
    // 死亡状态无法治疗
    public bool Heal(int value)
    {
        if (value < 0)
        {
            Debug.LogWarning("Heal value can't be negative.", this);
            return false;
        }
        if (IsDead) return false;

        int prev = Current;
        Current += value;
        Current = Mathf.Clamp(Current, 1, max);

        int change = Current - prev;
        if (change != 0)
        {
            Changed?.Invoke(new HealthChangedInfo(prev, Current));
        }
        return true;
    }

    public bool Revive(int health)
    {
        if (health <= 0)
        {
            Debug.LogWarning("Revive health must be above 0.", this);
            return false;
        }
        if (!IsDead) return false;

        Current = Mathf.Clamp(health, 1, max);
        IsDead = false;

        Changed?.Invoke(new HealthChangedInfo(0, Current));
        Revived?.Invoke();
        return true;
    }

    // 不触发Revived事件
    public void ResetToInit()
    {
        if (Current != init)
        {
            int prev=  Current;
            Current = init;
            Changed?.Invoke(new HealthChangedInfo(prev, Current));
        }
    }
}