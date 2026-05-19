using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WaterVolume : MonoBehaviour
{
    public float SurfaceY => transform.position.y + waterCollider.bounds.extents.y;

    private Collider waterCollider;

    private void Awake()
    {
        waterCollider = GetComponent<Collider>();
        waterCollider.isTrigger = true;
    }
}