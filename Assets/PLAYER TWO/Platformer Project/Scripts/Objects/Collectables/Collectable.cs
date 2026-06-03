using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] protected int value = 1;
    [SerializeField] private GameObject visual;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private bool hideOnCollect = true;
    [SerializeField] private bool collectOnContact = true;

    private AudioSource audioSource;
    private bool collected = false;

    private void OnValidate()
    {
        if (value < 0) value = 1;
    }

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public virtual void Collect()
    {
        if (collected) return;
        collected = true;
        
        if (collectSound)
        {
            audioSource.PlayOneShot(collectSound);
        }

        if (collectEffect)
        {
            collectEffect.Play();
        }

        if (hideOnCollect)
        {
            visual.SetActive(false);
        }
    }
    protected virtual bool CanCollect(Collider other)
    {
        return collectOnContact && !collected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanCollect(other))
        {
            Collect();
        }
    }
}