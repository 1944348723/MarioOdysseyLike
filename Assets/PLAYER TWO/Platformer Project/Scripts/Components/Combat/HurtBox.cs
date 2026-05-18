using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] private DamageReceiver damageReceiver;
    [SerializeField] private HitResponseType hitResponseType = HitResponseType.None;

    private void Awake()
    {
        if (!damageReceiver)
        {
            Debug.LogError("HurtBox was not binded to DamageReceiver.");
        }
    }

    public HitResponseType ReceiveHit(DamageInfo info)
    {
        if (damageReceiver) damageReceiver.TakeDamage(info);
        return hitResponseType;
    }
}