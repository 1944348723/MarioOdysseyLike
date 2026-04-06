using UnityEngine;

class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Player player;
    [SerializeField] float maxDistance = 15f;   // 相机与目标的最大距离
    [SerializeField] float initialPitch = 20f;

    [Header("Orbit Settings")]
    [SerializeField] private bool allowOrbitByInput = true;  // 是否允许通过输入环绕玩家
    [SerializeField] private float inputOrbitSpeed = 1f;  // 用户输入让相机环绕玩家的速度
    [SerializeField] private bool allowOrbitWithVelocity = true;  // 是否允许根据玩家速度自动环绕
    [SerializeField] private float velocityOrbitSpeed = 5f;  // 玩家速度让相机环绕的速度
    [Range(0, 90)]
    [SerializeField] private float maxPitch = 80f;      // 最大俯角
    [Range(-90, 0)]
    [SerializeField] private float minPitch = -20f;     // 最大仰角

    private const string FollowTargetName = "CameraFollowTarget";

    private Camera playerCamera;
    private CameraCollider cameraCollider;
    private Transform followTarget;
    private float yaw;  // 绕y轴旋转的角度
    private float pitch;  // 绕x轴旋转的角度

    private void Awake()
    {
        if (!player)
        {
            player = FindAnyObjectByType<Player>();
        }
        playerCamera = Camera.main;
        cameraCollider = playerCamera.GetComponent<CameraCollider>();
    }

    private void Start()
    {
        InitTarget();
        InitOrbitAngles();
        UpdateCamera();
    }

    private void LateUpdate() {
        OrbitByInput();
        OrbitWithVelocity();

        UpdateTarget();
        UpdateCamera();
        SolveCameraCollision();
    }

    private void InitTarget()
    {
        followTarget = new GameObject(FollowTargetName).transform;
        followTarget.position = player.transform.position;
    }

    private void InitOrbitAngles()
    {
        yaw = player.transform.eulerAngles.y;
        pitch = initialPitch;
    }

    private void UpdateCamera() {
        // 将(0, 0, -distance)绕x轴旋转pitch，绕y轴旋转yaw，得到相机相对玩家的偏移
        Vector3 backwardOffset = new(0, 0, -maxDistance);
        Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0);
        playerCamera.transform.position = followTarget.position + orbitRotation * backwardOffset;
        playerCamera.transform.LookAt(followTarget);
    }

    private void UpdateTarget()
    {
        followTarget.position = player.transform.position;
    }

    /// <summary>
    /// 根据输入让相机环绕玩家
    /// </summary>
    private void OrbitByInput()
    {
        if (!allowOrbitByInput) return;

        Vector2 lookDelta = player.Input.GetLookDelta();
        if (lookDelta == Vector2.zero) return;

        bool usingMouse = player.Input.IsLookingWithMouse();
        // 鼠标输入跟帧率无关，用户一帧内输入多少就是多少
        // 手柄输入是每帧根据偏移大小给出数值，帧率越大相同时间内得到的delta总和就越大，所以乘以Time.deltaTime让它跟帧率无关
        float timeFactor = usingMouse ? 1: Time.deltaTime;
        yaw += lookDelta.x * inputOrbitSpeed * timeFactor;
        pitch -= lookDelta.y * inputOrbitSpeed * timeFactor;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    // TODO: 手动环绕和自动环绕可能有一定冲突，后续可以在手动环绕时暂时禁用自动环绕，或者抑制自动环绕
    private void OrbitWithVelocity() {
        // 不在地上时不自动环绕，避免空中漂浮时乱转相机
        if (!allowOrbitWithVelocity || !player.IsGrounded) return;
        if (player.Input.GetLookDelta().sqrMagnitude > 0) return;

        // 得到玩家水平速度向相机水平方向右边的投影，如果是正的说明玩家向相机右边移动，负的说明玩家向相机左边移动
        // 根据这个投影调整yaw，让相机环绕玩家，转向玩家移动的方向
        Vector3 cameraPlanarForward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
        // Unity使用左手定则
        Vector3 cameraPlanarRight = Vector3.Cross(Vector3.up, cameraPlanarForward).normalized;
        // 带符号速度
        float speedAlongCameraPlanarRight = Vector3.Dot(player.PlanarVelocity, cameraPlanarRight);
        yaw += speedAlongCameraPlanarRight * velocityOrbitSpeed * Time.deltaTime;
    }

    private void SolveCameraCollision()
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        cameraCollider.ApplyCollider(ref cameraPosition, followTarget.position, maxDistance);
        playerCamera.transform.position = cameraPosition;
    }
}