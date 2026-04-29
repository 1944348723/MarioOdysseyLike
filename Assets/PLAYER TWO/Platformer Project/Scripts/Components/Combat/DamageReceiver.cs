using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageReceiver : MonoBehaviour, IDamageable
{
    public event Action<DamageInfo> Damaged;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void TakeDamage(DamageInfo info)
    {
        if (health.Damage(info.value))
        {
            Damaged?.Invoke(info);
        }
    }
}