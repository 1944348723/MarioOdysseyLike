using UnityEngine;

public class FlameTrap : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Collider flameCollider;
    [SerializeField] private ParticleSystem effect;

    private void Awake()
    {
        if (!flameCollider)
        {
            flameCollider = GetComponent<Collider>();
        }
    }

    private void Start()
    {
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
        effect.Play();
        flameCollider.enabled = true;
    }

    private void Deactivate()
    {
        effect.Stop();
        flameCollider.enabled = false;
    }
}