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
        float finalAcceleration = Stats.Current.acceleration * AccelerationMultiplier;
        float finalTurningDrag = Stats.Current.turningDrag * TurningDragMultiplier;
        float finalMaxSpeed = Stats.Current.maxSpeed * MaxSpeedMultiplier;
        base.Accelerate(direction.normalized, finalAcceleration, finalTurningDrag, finalMaxSpeed);
    }

    public void FaceToDirectionSmoothly(Vector3 direction)
    {
        base.FaceToDirection(direction, Stats.Current.rotationSpeed);
    }
}