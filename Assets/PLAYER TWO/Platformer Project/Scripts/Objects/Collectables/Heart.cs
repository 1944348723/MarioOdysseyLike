using UnityEngine;

public class Heart : Collectable
{
    private Player player;

    public override void Collect()
    {
        base.Collect();
        if (!player) return;

        Health health = player.GetComponent<Health>();
        health.Heal(value);
    }

    protected override bool CanCollect(Collider other)
    {
        return base.CanCollect(other) && other.TryGetComponent<Player>(out player);
    }
}