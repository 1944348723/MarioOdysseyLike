using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputSystem : Singleton<GameInputSystem>
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float jumpBufferTime = 0.15f;

    public bool MoveLocked { get; set; } = false;

    private const string MOUSE_DEVICE_NAME = "Mouse";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction crouchAndCrawlAction;
    private InputAction dashAction;
    private InputAction stompAction;
    private InputAction spinAction;
    private InputAction airDiveAction;
    private InputAction diveAction;
    private InputAction glideAction;
    private InputAction pauseAction;

    private Camera playerCamera;
    private float lastJumpPressedTime = -999f;

    override protected void Awake()
    {
        base.Awake();
        if (inputActions == null)
        {
            Debug.LogError("Input Actions is not assigned", this);
        }
        moveAction = inputActions?["Movement"];
        lookAction = inputActions?["Look"];
        jumpAction = inputActions?["Jump"];
        crouchAndCrawlAction = inputActions?["Crouch"];
        dashAction = inputActions?["Dash"];
        stompAction = inputActions?["Stomp"];
        spinAction = inputActions?["Spin"];
        airDiveAction = inputActions?["AirDive"];
        diveAction = inputActions?["Dive"];
        glideAction = inputActions?["Glide"];
        pauseAction = inputActions?["Pause"];

        playerCamera = Camera.main;
    }

    private void Start()
    {
        jumpAction.started += OnJumpPressed;
    }

    override protected void OnDestroy()
    {
        base.OnDestroy();
        jumpAction.started -= OnJumpPressed;
    }

    private void OnEnable()
    {
        inputActions?.Enable();
    }

    private  void OnDisable()
    {
        inputActions?.Disable();
    }

    /// <summary>
    /// 第三人称中，玩家的移动意图通常以“相机水平前方”为参考。
    /// 输入系统给的是平面输入向量（x/z），本身不包含相机朝向信息，是在世界默认前方为前向的坐标系下的
    /// 因此要用相机的 yaw（绕世界 up 轴）去旋转输入向量，得到世界空间下的移动方向。
    /// Quaternion.AngleAxis 用“绕指定轴旋转指定角度”的方式构造这个旋转。
    /// </summary>
    public Vector3 GetMoveDirectionBasedOnCamera()
    {
        Vector3 inputDirection = GetMovementDirection();
        if (inputDirection == Vector3.zero) return inputDirection;

        // 绕竖直向上方向(y轴)旋转camera的y轴欧拉角那么多
        // 相当于把输入坐标系从世界默认前方转到了相机水平前方
        var rotation = Quaternion.AngleAxis(this.playerCamera.transform.eulerAngles.y, Vector3.up);
        Vector3 direction = rotation * inputDirection;
        
        direction.Normalize();
        return direction;
    }

    public Vector3 GetMovementDirection()
    {
        if (MoveLocked) return Vector3.zero;

        Vector2 inputValue = moveAction.ReadValue<Vector2>();
        Vector2 processedInput = GetAxisWithCrossDeadZone(inputValue);
        return new Vector3(processedInput.x, 0, processedInput.y);
    }

    public Vector2 GetLookDelta()
    {
        Vector2 input = lookAction.ReadValue<Vector2>();
        if (IsLookingWithMouse())
        {
            return input;
        } else
        {
            return GetAxisWithCrossDeadZone(input);
        }
    }

    public bool IsLookingWithMouse()
    {
        if (lookAction.activeControl == null) return false;
        return lookAction.activeControl.device.name.Equals(MOUSE_DEVICE_NAME);
    }

    // 带缓冲
    public bool HasBufferedJump()
    {
        return Time.time - lastJumpPressedTime < jumpBufferTime;
    }

    public void ConsumeBufferedJump()
    {
        lastJumpPressedTime = -999f;
    }

    public void LockCamera()
    {
        lookAction?.Disable();
    }

    public void UnlockCamera()
    {
        lookAction?.Enable();
    }

    public bool IsJumpReleasedThisFrame() => jumpAction.WasReleasedThisFrame();
    public bool IsCrouchAndCrawlPressed() => crouchAndCrawlAction.IsPressed();
    public bool IsCrouchPressed() => crouchAndCrawlAction.IsPressed();
    public bool IsDashPressedThisFrame() => dashAction.WasPressedThisFrame();
    public bool IsStompPressedThisFrame() => stompAction.WasPressedThisFrame();
    public bool IsSpinPressedThisFrame() => spinAction.WasPressedThisFrame();
    public bool IsAirDivePressedThisFrame() => airDiveAction.WasPressedThisFrame();
    public bool IsDivePressed() => diveAction.IsPressed();
    public bool IsGlidePressed() => glideAction.IsPressed();
    public bool IsPausePressedThisFrame() => pauseAction.WasPressedThisFrame();

    private Vector2 GetAxisWithCrossDeadZone(Vector2 axis)
    {
        float deadZone = InputSystem.settings.defaultDeadzoneMin;
        float len = axis.magnitude;
        if (len <= deadZone) return Vector2.zero;

        float remapedLen = (len - deadZone) / (1 - deadZone);
        Vector2 dir = axis / len;
        return dir * remapedLen;
    }

    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        lastJumpPressedTime = Time.time;
    }

}
