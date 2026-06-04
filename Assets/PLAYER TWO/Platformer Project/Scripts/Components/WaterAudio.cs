using UnityEngine;

public class WaterAudio : MonoBehaviour
{
    [SerializeField] AudioClip enterWaterClip;
    [SerializeField] AudioClip exitWaterClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WaterVolume>())
        {
            audioSource.PlayOneShot(enterWaterClip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<WaterVolume>())
        {
            audioSource.PlayOneShot(exitWaterClip);
        }
    }
}