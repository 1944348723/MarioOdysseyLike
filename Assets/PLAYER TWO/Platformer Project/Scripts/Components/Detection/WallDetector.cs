using UnityEngine;

public class WallDetector : MonoBehaviour {
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float heightOffset = 0f;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private float detectDistance = 0.01f;
    [SerializeField] private float maxNormalYAbs = 0.1f;

    public bool HasWall { get; private set; }
    public Vector3 WallNormal { get; private set; }
    public Vector3 WallPoint { get; private set; }

    private void OnValidate()
    {
        if (radius < 0) radius = 0.1f;
        if (detectDistance <= 0) detectDistance = 0.1f;
        if (maxNormalYAbs < 0) maxNormalYAbs = 0f;
        else if (maxNormalYAbs > 1) maxNormalYAbs = 1f;
    }

    public void Check(Vector3 direction) {
        direction.Normalize();

        Vector3 origin = transform.position + new Vector3(0, heightOffset, 0);
        bool hitWall = Physics.SphereCast(
            origin,
            radius,
            direction,
            out RaycastHit hit,
            detectDistance,
            wallLayer,
            QueryTriggerInteraction.Ignore
        );

        HasWall = false;
        WallNormal = Vector3.zero;
        WallPoint = Vector3.zero;
        if (hitWall && IsValid(hit.normal))
        {
            HasWall = true;
            WallNormal = hit.normal;
            WallPoint = hit.point;
        }
    }
    
    public bool IsDirectionTowardWall(Vector3 direction)
    {
        if (!HasWall) return false;

        direction.Normalize();
        float cos = Vector3.Dot(direction, WallNormal.normalized);
        return cos < 0;
    }

    private bool IsValid(Vector3 normal)
    {
        return Mathf.Abs(normal.y) <= maxNormalYAbs;
    }
}