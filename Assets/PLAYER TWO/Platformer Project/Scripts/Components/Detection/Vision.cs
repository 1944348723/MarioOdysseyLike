using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private GameObject target;
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private float viewAngle = 90f;

    public bool CanSeeTarget { get; private set; }
    public Transform Target => target.transform;

    private Collider[] hits = new Collider[64];

    private void OnValidate()
    {
        if (viewDistance < 0) viewDistance = 0f;
    }

    private void Update()
    {
        Scan();
    }

    private void Scan()
    {
        CanSeeTarget = false;

        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            viewDistance,
            hits,
            targetLayers,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < count; ++i)
        {
            var hit = hits[i];
            if (hit.gameObject != target)
            {
                continue;
            }

            Vector3 toTarget = target.transform.position - transform.position;
            if (!(Vector3.Angle(transform.forward, toTarget) < viewAngle * 0.5f))
            {
                return;
            }

            if (Physics.Raycast(transform.position, toTarget.normalized, out RaycastHit rayHit,
                viewDistance, targetLayers | obstacleLayers, QueryTriggerInteraction.Ignore))
            {
                if (rayHit.collider.gameObject == target)
                {
                    CanSeeTarget = true;
                }
                return;
            }
            return;
        }
    }
}