using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{
    [SerializeField] private float maxForce = 10f;
    [SerializeField] private float maxForceDepth = 1f;
    [SerializeField] private float waterDrag = 2f;
    [SerializeField] private float waterAngularDrag = 0.2f;


    private Rigidbody rb;
    private WaterVolume currentWater;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!currentWater) return;

        float depth = currentWater.SurfaceY - transform.position.y;
        float ratio = Mathf.Clamp01(depth / maxForceDepth);
        if (ratio <= 0) return;

        rb.AddForce(maxForce * ratio * Vector3.up);
        rb.AddForce(ratio * waterDrag * -rb.velocity);
        rb.AddTorque(ratio * waterAngularDrag * -rb.angularVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<WaterVolume>(out var water))
        {
            currentWater = water;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<WaterVolume>(out var water))
        {
            currentWater = null;
        }
    }
}