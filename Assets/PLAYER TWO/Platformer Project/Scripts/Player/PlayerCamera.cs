using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] float maxDistance = 15f;   // 相机与目标的最大距离
    [SerializeField] float initialPitch = 20f;
    [SerializeField] float hightOffset = 1f;

    private const string TargetName = "CameraTarget";

    private CinemachineVirtualCamera virtualCamera;
    private Cinemachine3rdPersonFollow virtualCameraBody;   // 相机跟随逻辑
    private CinemachineBrain virtualCameraBrain;
    private Transform target;   // 相机跟随目标，是单独创建的，持续同步到player的位置上

    private void Awake()
    {
        if (!player)
        {
            player = FindAnyObjectByType<Player>();
        }
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraBody = virtualCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
        virtualCameraBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        InitTarget();
        InitVirtualCamera();
    }

    private void InitTarget()
    {
        target = new GameObject(TargetName).transform;
        target.position = player.transform.position;
    }

    private void InitVirtualCamera()
    {
        virtualCamera.Follow = target;
        virtualCamera.LookAt = player.transform;

        Reset();
    }

    private void Reset()
    {
        Vector3 initPosition = player.transform.position + Vector3.up * hightOffset;
        float initYaw = player.transform.eulerAngles.y;
        MoveTarget(initPosition, Quaternion.Euler(initialPitch, initYaw, 0));

        virtualCameraBody.CameraDistance = maxDistance;

        virtualCameraBrain.ManualUpdate();
    }

    private void MoveTarget(Vector3 position, Quaternion rotation)
    {
        target.SetPositionAndRotation(position, rotation);
    }
}