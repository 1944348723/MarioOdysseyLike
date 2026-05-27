using UnityEngine;

public class PlayerStats : EntityStats<PlayerStats>
{
    [Header("General Stats")]
    public float pushForce = 4f;                    // 推动物体的力
    public float snapSpeed = 15f;                   // 将角色贴合到地面的速度
    public float slideForce = 10;                   // 下坡滑动的额外推力
    public float rotationSpeed = 970f;              // 玩家角色转动速度(°/s)
    public float gravity = 38f;                     // 普通重力加速度
    public float fallGravity = 65f;                 // 下落时额外重力加速度
    public float maxFallingSpeed = 50f;             // 重力作用下的最大下落速度

    [Header("Motion Stats")]
    public bool applySlopeFactor = true;            // 是否考虑坡度因子
    public float acceleration = 13f;                // 加速度
    public float deceleration = 28f;                // 减速度
    public float friction = 28f;                    // 地面摩擦力
    public float slopeFriction = 18f;               // 坡面摩擦力
    public float maxSpeed = 6f;                     // 最高速度
    public float turningDrag = 28f;                 // 转向时的阻力
    public float airAcceleration = 32f;             // 空中加速度
    public float brakeThreshold = -0.8f;            // 刹车判定阈值
    public float slopUpwordForce = 25f;             // 上坡时的额外推力
    public float slopDownwordForce = 28f;           // 下坡时的额外推力

    [Header("Jump Stats")]
    public int allowedJumpTimes = 2;                // 允许跳跃次数
    public float coyoteJumpThreshold = 0.15f;       // 土狼跳判定时间阈值
    public float maxJumpSpeed = 17f;
    public float minJumpSpeed = 10f;

    [Header("Hurt Stats")]
    public float hurtBackwardSpeed = 5f;
    public float hurtUpwardSpeed = 10f;

    [Header("Crouch Stats")]
    public float crouchHeight = 1f;
    public float crouchFriction = 10f;

    [Header("Crawl Stats")]
    public float crawlAcceleration = 8f;
    public float crawlFriction = 32f;
    public float crawlMaxSpeed = 2.5f;
    public float crawlTurningSpeed = 3f;

    [Header("Backflip Stats")]
    public bool canBackFlip = true;
    public bool lockMovementDuringBackflip = true;
    public float backflipUpwardSpeed = 23f;
    public float backflipBackwardSpeed = 4f;
    public float backflipGravity = 35f;
    public float backflipAirAcceleration = 12f;
    public float backflipAirTurningSpeed = 2.5f;
    public float backflipAirMaxSpeed = 7.5f;

    [Header("Dash Stats")]
    public bool canGroundDash = true;
    public bool canAirDash = true;
    public float dashSpeed = 25f;
    public float dashDuration = 0.3f;
    public float dashCoolDown = 0.5f;

    [Header("Stomp Attack Stats")]
    public bool canStompAttack = true;
    public float stompHoverDuration = 0.8f;
    public float stompRecoveryDuration = 0.5f;
    public float stompDownSpeed = 50f;
    public float stompBounceSpeed = 10f;

    [Header("Spin Stats")]
    public bool canSpin = true;
    public bool canAirSpin = true;
    public float spinDuration = 0.5f;
    public float airSpinUpwardSpeed = 10f;
    public int allowedAirSpinTimes = 1;
    public float spinBounceSpeed = 5f;

    [Header("Air Dive Stats")]
    public bool canAirDive = true;
    public float airDiveForwardSpeed = 16f;
    public float airDiveFriction = 32f;
    
    [Header("Swim Stats")]
    public float swimEnterThreshold = 0f;
    public float swimAcceleration = 4f;
    public float swimTurningDrag = 2.5f;
    public float swimMaxSpeed = 4f;
    public float swimDeceleration = 3f;
    public float swimJumpSpeed = 15f;
    public float swimUpwardAcceleration = 2f;
    public float swimUpwardMaxSpeed = 4f;
    public float swimDiveAcceleration = 1f;
    public float swimDiveMaxSpeed = 2.5f;

    [Header("Glide Stats")]
    public bool canGlide = true;
    public float glideGravity = 10f;
    public float glideMaxFallingSpeed = 2f;
    public float glideTurningDrag = 8f;

    [Header("Wall Slide Stats")]
    public bool canWallSlide = true;
    public float wallSlideSpeed = 6f;
    public float wallJumpVerticalSpeed = 15f;
    public float wallJumpPlanarSpeed = 8f;

    [Header("Pole Climb Stats")]
    public bool canPoleClimb = true;
    public float poleClimbRegrabCoolDown = 0.1f;
    public float poleClimbUpSpeed = 3f;
    public float poleClimbDownSpeed = 8f;
    public float poleClimbRatationSpeed = 2f;
    public float poleClimbJumpVerticalSpeed = 15f;
    public float poleClimbJumpPlanarSpeed = 8f;
}