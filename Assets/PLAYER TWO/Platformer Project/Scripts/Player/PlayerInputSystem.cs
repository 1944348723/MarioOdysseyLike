using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputAction moveAction;
    private Camera camera;

    private void Awake()
    {
        if (inputActions == null)
        {
            Debug.LogError("Input Actions is not assigned", this);
        }
        moveAction = inputActions?["Movement"];

        camera = Camera.main;
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
        if (inputDirection.sqrMagnitude == 0) return inputDirection;

        // 绕竖直向上方向(y轴)旋转camera的y轴欧拉角那么多
        // 相当于把输入坐标系从世界默认前方转到了相机水平前方
        var rotation = Quaternion.AngleAxis(this.camera.transform.eulerAngles.y, Vector3.up);
        Vector3 direction = rotation * inputDirection;
        
        direction.Normalize();
        return direction;
    }

    public Vector3 GetMovementDirection()
    {
        Vector2 inputValue = moveAction.ReadValue<Vector2>();
        return GetAxisWithCrossDeadZone(inputValue);
    }

    private Vector3 GetAxisWithCrossDeadZone(Vector2 axis)
    {
        float deadZone = InputSystem.settings.defaultDeadzoneMin;
        float len = axis.magnitude;
        if (len <= deadZone) return Vector3.zero;

        float remapedLen = (len - deadZone) / (1 - deadZone);
        Vector2 dir = axis / len;
        return new Vector3(dir.x * remapedLen, 0, dir.y * remapedLen);
    }
}
