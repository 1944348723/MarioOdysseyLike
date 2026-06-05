using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal exit;
    [SerializeField] private Vector3 exitOffset;
    [SerializeField] private Collider portalCollider;
    [SerializeField] private AudioClip clip;

    public Vector3 ExitPosition() => transform.position + exitOffset;

    private AudioSource audioSource;

    private void Awake()
    {
        if (!portalCollider)
        {
            portalCollider = GetComponent<Collider>();
        }
        portalCollider.isTrigger = true;

        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!exit || !other.TryGetComponent(out Player player)) return;

        // 移动
        float yOffset = player.transform.position.y - transform.position.y;
        player.transform.position = exit.ExitPosition() + yOffset * Vector3.up;

        // 朝向
        Vector3 planardirectionToExitPortal = exit.transform.position - player.transform.position;
        planardirectionToExitPortal.y = 0;
        player.transform.forward = -planardirectionToExitPortal;

        // 速度
        player.PlanarVelocity = -planardirectionToExitPortal * player.PlanarVelocity.magnitude;

        if (clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}