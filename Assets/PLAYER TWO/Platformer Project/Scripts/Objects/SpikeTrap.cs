using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private Button3D button;
    [SerializeField] private Collider spikeCollider;
    [SerializeField] private Vector3 deactivateOffset = new(0, -1, 0);
    [SerializeField] private float animationDuration = 0.2f;

    private Vector3 activatePosition;
    private Vector3 deactivatePosition;

    private void Awake()
    {
        if (!spikeCollider)
        {
            spikeCollider = GetComponent<Collider>();
        }
    }

    private void Start()
    {
        activatePosition = transform.position;
        deactivatePosition = transform.position + deactivateOffset;
        button.Pressed += Deactivate;
        button.Released += Activate;
    }

    private void OnDestroy()
    {
        button.Pressed -= Deactivate;
        button.Released -= Activate;
    }

    private void Activate()
    {
        spikeCollider.enabled = true;
        DoTween.To<Vector3>(
            () => transform.position,
            position => transform.position = position,
            activatePosition,
            animationDuration
        );
    }

    private void Deactivate()
    {
        spikeCollider.enabled = false;
        DoTween.To<Vector3>(
            () => transform.position,
            position => transform.position = position,
            deactivatePosition,
            animationDuration
        );
    }
}