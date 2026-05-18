using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Collider hitCollider;

    public event Action<HitResponseType, Vector3> hit;

    private void OnValidate()
    {
        if (damage <= 0) damage = 1;
    }

    private void Awake()
    {
        if (!hitCollider)
        {
            hitCollider = GetComponent<Collider>();
        }
        hitCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryAttack(other.gameObject);
    }

    private void TryAttack(GameObject other)
    {
        if (!other.TryGetComponent<HurtBox>(out var hurtBox)) return;

        DamageInfo info = new()
        {
            value = damage,
            sourcePosition = transform.position
        };
        HitResponseType hitResponseType = hurtBox.ReceiveHit(info);
        hit?.Invoke(hitResponseType, other.transform.position);
    }
}