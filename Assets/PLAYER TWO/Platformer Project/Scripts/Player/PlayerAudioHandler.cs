using UnityEngine;

[RequireComponent(typeof(WaterDetector))]
public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioClip enterWaterClip;
    [SerializeField] private AudioClip exitWaterClip;
    [SerializeField] private AudioClip glideStartedClip;
    [SerializeField] private AudioClip glideEndedClip;

    private AudioSource audioSource;
    private WaterDetector waterDetector;
    private Player player;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        waterDetector = GetComponent<WaterDetector>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        waterDetector.EnteredWater += OnEnteredWater;
        waterDetector.ExitedWater += OnExitedWater;
        player.playerEvents.GlideStarted.AddListener(OnGlideStarted);
        player.playerEvents.GlideEnded.AddListener(OnGlideEnded);
    }

    private void OnDisable()
    {
        waterDetector.EnteredWater -= OnEnteredWater;
        waterDetector.ExitedWater -= OnExitedWater;
        player.playerEvents.GlideStarted.RemoveListener(OnGlideStarted);
        player.playerEvents.GlideEnded.RemoveListener(OnGlideEnded);
    }

    private void OnEnteredWater(WaterVolume water)
    {
        audioSource.PlayOneShot(enterWaterClip);
    }

    private void OnExitedWater(WaterVolume water)
    {
        audioSource.PlayOneShot(exitWaterClip);
    }

    private void OnGlideStarted()
    {
        audioSource.PlayOneShot(glideStartedClip);
    }

    private void OnGlideEnded()
    {
        audioSource.PlayOneShot(glideEndedClip);
    }
}