using System;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private float height;
    [SerializeField] private float radius;
    // 如果为0的话，检测边界刚好贴着碰撞胶囊最低位置，可能刚好站在地上但判定为离地
    // 还有浮点误差、skinWidth、碰撞修正、地面不平整之类的，判定可能一帧离地，一帧接地
    // 下坡、下台阶时，往前走了一点就判定离地，掉下来了又判定接地
    // goundOffset可以把这些一点点的高度差给吞掉，增加容错和检测稳定性
    // 当然也不能太大了，太大会过早判定为触地，会过早切换动画、可以跳跃之类的
    // 在比较小的范围内就可以接受，看不出什么问题，还能解决上述的那些问题
    [SerializeField] private float groundOffset = 0.1f;
    // 如果没有slopeLimit限制，那任意斜面都会判定是地面
    // 比如有个建筑墙是弧形的，但是坡度其实还是很大，没有slopelimit往墙上跳，触墙会判定为落地，这样肯定是不对的
    [SerializeField] private float slopeLimit = 0;
    // 如果没有stepOffset限制，那么只要胶囊体底部碰到东西，并满足坡度要求，就判定为接地
    // 假如有一个平台边缘是垂直角，人物站在边缘一点点往外蹭
    // 蹭到后面，只有胶囊体底部非常靠侧面的一个点触地，也判定为接地，这时候人物已经基本完全走出平台了
    // 添加一个stepOffset，就相当于在胶囊体最最低点往上stepOffset高度拉一条线，如果碰撞点在这条线之上的，都不算触地, 这样就能缓解前面所说的问题
    // 不过如果加了stepOffset，不要设置的过小，否则判定过于严格，比如0的时候，只有碰撞点低于胶囊体最底部才算触地
    [SerializeField] private float stepOffset = 0;

    public bool IsGrounded { get; private set; }
    public bool IsOnSlope { get; private set; }
    public float LastGoundedTime { get; private set; }

    public Action GroundEntered;
    public Action GroundExited;

    // 运行时数据
    private Vector3 stepPosition;
    private RaycastHit groundHit;
    private float groundAngle;
    private Vector3 groundNormal;
    private Vector3 localSlopeDirection;

    public void Init(float height, float radius, float groundOffset = 0.1f,
        float slopeLimit = 90, float stepOffset = 0)
    {
        if (height < 0) throw new ArgumentOutOfRangeException(nameof(height));
        if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius));
        if (groundOffset < 0) throw new ArgumentOutOfRangeException(nameof(groundOffset));
        if (slopeLimit < 0 || slopeLimit > 90) throw new ArgumentOutOfRangeException(nameof(slopeLimit));
        if (stepOffset < 0) throw new ArgumentOutOfRangeException(nameof(stepOffset));

        this.height = height;
        this.radius = radius;
        this.groundOffset = groundOffset;
        this.slopeLimit = slopeLimit;
        this.stepOffset = stepOffset;
    }

    private void OnValidate()
    {
        if (height < 0) height = 0;
        if (radius < 0) radius = 0;
        if (groundOffset < 0) groundOffset = 0;
        if (stepOffset < 0) stepOffset = 0;
        if (slopeLimit < 0 || slopeLimit > 90) slopeLimit = 90;
    }

    public void Tick(Vector3 detectOrigin, bool canEnterGround = true)
    {
        this.stepPosition = detectOrigin - Vector3.up * (height * 0.5f - stepOffset);

        // 检测距离
        float distance = height * 0.5f + groundOffset;
        bool groundDetected = SphereCast(detectOrigin, Vector3.down, distance, out var hit);

        // 不是检测到地面就是着地，还可能有额外条件，比如角色不能在上升，也就是起跳后哪怕没有离地也算离地
        if (!groundDetected || !canEnterGround || !IsValidGround(hit))
        {
            ExitGround();
            return;
        }

        if (!IsGrounded)
        {
            EnterGround(hit);
        } else
        {
            UpdateGround(hit);
        }
    }

    private bool SphereCast(Vector3 origin, Vector3 direction, float detectDistance,
        out RaycastHit hit, int layer = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
    {
        // 发射的是球体，发射原点是球心所在位置，所以detectDistance - Radius作为最大投射距离，才正好检测detectDistance那么长
        // 但是这里为负的时候的处理感觉不是很好
        float castDistance = Mathf.Abs(detectDistance - radius);

        return Physics.SphereCast(origin, radius, direction, out hit, 
            castDistance, layer, queryTriggerInteraction);
    }

    private bool IsPointUnderStep(Vector3 point) {
        return point.y < stepPosition.y;
    }

    private bool IsValidGround(RaycastHit hit)
    {
        float slope = Vector3.Angle(hit.normal, Vector3.up);
        return IsPointUnderStep(hit.point) && slope < slopeLimit;
    }
    
    private void EnterGround(RaycastHit hit)
    {
        if (!IsGrounded)
        {
            IsGrounded = true;
            UpdateGround(hit);
            GroundEntered?.Invoke();
        }
    }
    
    private void ExitGround()
    {
        if (IsGrounded)
        {
            IsGrounded = false;
            GroundExited?.Invoke();
        }
    }
    
    private void UpdateGround(RaycastHit hit)
    {
        if (IsGrounded)
        {
            LastGoundedTime = Time.time;
            groundHit = hit;
            groundNormal = hit.normal;
            groundAngle = Vector3.Angle(hit.normal, Vector3.up);
            localSlopeDirection = new Vector3(groundNormal.x, 0, groundNormal.z).normalized;
        }
    }
}