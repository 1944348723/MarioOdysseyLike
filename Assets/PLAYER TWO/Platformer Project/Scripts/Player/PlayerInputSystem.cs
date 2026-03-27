using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputAction moveAction;

    private void Awake()
    {

        if (inputActions == null)
        {
            Debug.LogError("Input Actions is not assigned", this);
        }
        moveAction = inputActions?["Movement"];
    }

    private void OnEnable()
    {
        inputActions?.Enable();
    }

    private  void OnDisable()
    {
        inputActions?.Disable();
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
