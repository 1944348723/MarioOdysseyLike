using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private AudioClip clip;

    public UnityEvent Activated;
    private AudioSource audioSource;
    private bool isActive = false;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive) return;
        if (!other.GetComponent<Player>()) return;
        
        isActive = true;
        if (clip)
        {
            audioSource.PlayOneShot(clip);
        }

        LevelManager.Instance.SetRespawnPoint(respawnPoint);

        Activated?.Invoke();
    }
}