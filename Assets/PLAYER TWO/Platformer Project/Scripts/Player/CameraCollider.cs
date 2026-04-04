using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField] private LayerMask collideAgainst;
    [SerializeField] private float radius = 0.2f;
    [Tooltip("相机返回速度，为0时瞬间拉回")]
    [SerializeField] private float returnSpeed = 10f;

    private float currentDistance = -1f;

    private void OnValidate()
    {
        if (radius < 0) radius = 0;
        if (returnSpeed < 0) returnSpeed = 0;
    }

    // 当前会有人物在墙边，将相机环绕到墙的方向时，会前移导致离人物过近，出现相机穿进人物的现象
    public void ApplyCollider(ref Vector3 cameraPosition, Vector3 aimPosition, float desiredDistance)
    {
        // 第一次进入时，相机距离目标为最大距离
        if (currentDistance < 0) currentDistance = desiredDistance;

        // 从目标指向相机的方向
        Vector3 toCamera = cameraPosition - aimPosition;
        float currentMagnitude = toCamera.magnitude;
        if (currentMagnitude < Mathf.Epsilon) return;
        Vector3 direction = toCamera / currentMagnitude;

        // 计算安全距离(相机不穿进障碍物)
        float safeDistance = desiredDistance;
        if (Physics.SphereCast(aimPosition, radius, direction,
                out RaycastHit hit, desiredDistance, collideAgainst))
        {
            safeDistance = hit.distance;
        }

        // 瞬间拉近
        if (safeDistance < currentDistance) {
            currentDistance = safeDistance;
        }
        else {
            if (returnSpeed == 0) {
                currentDistance = safeDistance;
            } else {
                currentDistance = Mathf.MoveTowards(currentDistance, safeDistance, returnSpeed * Time.deltaTime);
            }
        }

        cameraPosition = aimPosition + direction * currentDistance;
    }
}
