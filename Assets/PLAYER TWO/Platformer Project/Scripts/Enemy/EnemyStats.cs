using UnityEngine;

public class EnemyStats : EntityStats<EnemyStats>
{
    [Header("General Stats")]
    public float gravity = 38f;
    public float fallGravity = 65f;
    public float snapSpeed = 15f;
    public float rotationSpeed = 970f;
    public float deceleration = 28f;
    public float friction = 16f;
    public float turningDrag = 28f;
    public float maxFallingSpeed = 50f;

    [Header("Patrol Stats")]
    public float patrolAcceleration = 10f;
    public float patrolMaxSpeed = 1f;
    public float checkDistance = 0.5f;

    [Header("Chase Stats")]
    public float ChaseAcceleration = 10f;
    public float ChaseMaxSpeed = 4f;
}