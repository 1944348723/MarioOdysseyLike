using UnityEngine;

class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Player player;
    [SerializeField] float cameraDistance = 15f;   // 相机与目标的距离
    [SerializeField] float initialPitch = 20f;

    [Header("Orbit Settings")]
    [SerializeField] private bool allowOrbitByInput = true;  // 是否允许通过输入环绕玩家
    [SerializeField] private float inputOrbitSpeed = 1f;  // 用户输入让相机环绕玩家的速度
    [Range(0, 90)]
    [SerializeField] private float maxPitch = 80f;      // 最大俯角
    [Range(-90, 0)]
    [SerializeField] private float minPitch = -20f;     // 最大仰角

    private const string FollowTargetName = "CameraFollowTarget";

    private Camera playerCamera;
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
    }

    private void Start()
    {
        InitTarget();
        InitOrbitAngles();
        UpdateCamera();
    }

    private void LateUpdate() {
        OrbitByInput();
        UpdateTarget();
        UpdateCamera();
    }

    private void InitTarget()
    {
        followTarget = new GameObject(FollowTargetName).transform;
        followTarget.SetParent(transform);
        followTarget.position = player.transform.position;
    }

    private void InitOrbitAngles()
    {
        yaw = player.transform.eulerAngles.y;
        pitch = initialPitch;
    }

    private void UpdateCamera() {
        // 将(0, 0, -distance)绕x轴旋转pitch，绕y轴旋转yaw，得到相机相对玩家的偏移
        Vector3 backwardOffset = new(0, 0, -cameraDistance);
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
}