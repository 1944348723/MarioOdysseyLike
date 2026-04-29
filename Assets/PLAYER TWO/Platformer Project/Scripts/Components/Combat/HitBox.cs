using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Collider hitCollider;

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
            sourcePosition = other.transform.position
        };
        hurtBox.ReceiveHit(info);
    }
}