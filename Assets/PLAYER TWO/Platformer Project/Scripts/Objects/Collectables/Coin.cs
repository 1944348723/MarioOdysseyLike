using UnityEngine;

public class Coin : Collectable
{
    public override void Collect()
    {
        base.Collect();
        LevelManager.Instance.AddCoin(value);
    }

    protected override bool CanCollect(Collider other)
    {
        return base.CanCollect(other) && other.GetComponent<Player>();
    }
}