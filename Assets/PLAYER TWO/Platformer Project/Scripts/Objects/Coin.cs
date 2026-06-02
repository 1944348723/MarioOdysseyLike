using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private GameObject visual;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private ParticleSystem collectEffect;

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

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.GetComponent<Player>()) return;

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

        visual.SetActive(false);
    }
}