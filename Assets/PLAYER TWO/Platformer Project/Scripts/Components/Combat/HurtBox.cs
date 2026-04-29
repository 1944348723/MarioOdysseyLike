using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] private DamageReceiver damageReceiver;

    private void Awake()
    {
        if (!damageReceiver)
        {
            Debug.LogError("HurtBox was not binded to DamageReceiver.");
        }
    }

    public void ReceiveHit(DamageInfo info)
    {
        damageReceiver.TakeDamage(info);
    }
}