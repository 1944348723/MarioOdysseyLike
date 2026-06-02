using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
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

    public void Collect()
    {
        if (collected) return;
        collected = true;
        LevelManager.Instance.AddCoin(value);
        
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

    private void OnTriggerEnter(Collider other)
    {
        if (!collectOnContact) return;
        if (!other.GetComponent<Player>()) return;
        Collect();
    }
}