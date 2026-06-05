using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ForceField : MonoBehaviour
{
    [SerializeField] private Vector3 direction = new(0, 1, 0);
    [SerializeField] private float force = 75f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Player player)) return;

        if (player.IsGrounded)
        {
            player.VerticalVelocity = Vector3.zero;
        }
        player.Velocity += force * Time.deltaTime * direction;
    }
}