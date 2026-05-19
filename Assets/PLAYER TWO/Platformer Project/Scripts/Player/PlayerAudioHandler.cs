using UnityEngine;

[RequireComponent(typeof(WaterDetector))]
public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioClip enterWaterClip;
    [SerializeField] private AudioClip exitWaterClip;

    private AudioSource audioSource;
    private WaterDetector waterDetector;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        waterDetector = GetComponent<WaterDetector>();
    }

    private void OnEnable()
    {
        waterDetector.EnteredWater += OnEnteredWater;
        waterDetector.ExitedWater += OnExitedWater;
    }

    private void OnDisable()
    {
        waterDetector.EnteredWater -= OnEnteredWater;
        waterDetector.ExitedWater -= OnExitedWater;
    }

    private void OnEnteredWater(WaterVolume water)
    {
        audioSource.PlayOneShot(enterWaterClip);
    }

    private void OnExitedWater(WaterVolume water)
    {
        audioSource.PlayOneShot(exitWaterClip);
    }
}