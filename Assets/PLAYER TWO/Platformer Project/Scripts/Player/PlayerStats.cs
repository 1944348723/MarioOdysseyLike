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
}