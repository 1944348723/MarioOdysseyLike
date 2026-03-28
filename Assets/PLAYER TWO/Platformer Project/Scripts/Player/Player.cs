using UnityEngine;

public class Player : Entity<Player>
{
    public PlayerInputSystem Input { get; protected set; }
    public PlayerStatsManager Stats { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Input = GetComponent<PlayerInputSystem>();
        Stats = GetComponent<PlayerStatsManager>();
    }

    public void Accelerate(Vector3 direction)
    {
        base.Accelerate(direction.normalized, Stats.Current.acceleration, Stats.Current.turningDrag, Stats.Current.maxSpeed);
    }

    public void FaceToDirectionSmoothly(Vector3 direction)
    {
        base.FaceToDirection(direction, Stats.Current.rotationSpeed);
    }
}