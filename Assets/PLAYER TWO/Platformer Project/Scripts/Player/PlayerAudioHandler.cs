using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("Voices")]
    [SerializeField] private AudioClip[] jump;
    [SerializeField] private AudioClip[] hurt;
    [SerializeField] private AudioClip[] attack;
    [SerializeField] private AudioClip[] lift;
    [SerializeField] private AudioClip[] maneuver;

    [Header("Effects")]
    [SerializeField] private AudioClip enterWaterClip;
    [SerializeField] private AudioClip exitWaterClip;
    [SerializeField] private AudioClip glideStartedClip;
    [SerializeField] private AudioClip glideEndedClip;
    [SerializeField] private AudioClip spin;
    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip drop;
    [SerializeField] private AudioClip airDive;
    [SerializeField] private AudioClip stompSpin;
    [SerializeField] private AudioClip stompLanding;
    [SerializeField] private AudioClip ledgeGrabbing;
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip startRailGrind;
    [SerializeField] private AudioClip railGrind;

    private AudioSource audioSource;

    private void Awake()
    {
        if (!player)
        {
            player = FindAnyObjectByType<Player>();
        }
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        player.playerEvents.Jumped.AddListener(OnJumped);
        player.playerEvents.Hurt.AddListener(OnHurt);
        player.playerEvents.SpinStarted.AddListener(OnAttacked);
        player.playerEvents.Backfliped.AddListener(OnManeuver);

        player.PlayerWaterDetector.EnteredWater += OnEnteredWater;
        player.PlayerWaterDetector.ExitedWater += OnExitedWater;
        player.playerEvents.GlideStarted.AddListener(OnGlideStarted);
        player.playerEvents.GlideEnded.AddListener(OnGlideEnded);
        player.playerEvents.SpinStarted.AddListener(OnSpun);
        player.playerEvents.AirDived.AddListener(OnAirDive);
        player.playerEvents.StompLanded.AddListener(OnStompLanding);
        player.playerEvents.DashStarted.AddListener(OnDash);
    }

    private void OnDestroy()
    {
        player.playerEvents.Jumped.RemoveListener(OnJumped);
        player.playerEvents.Hurt.RemoveListener(OnHurt);
        player.playerEvents.SpinStarted.RemoveListener(OnAttacked);
        player.playerEvents.Backfliped.RemoveListener(OnManeuver);

        player.PlayerWaterDetector.EnteredWater -= OnEnteredWater;
        player.PlayerWaterDetector.ExitedWater -= OnExitedWater;
        player.playerEvents.GlideStarted.RemoveListener(OnGlideStarted);
        player.playerEvents.GlideEnded.RemoveListener(OnGlideEnded);
        player.playerEvents.SpinStarted.RemoveListener(OnSpun);
        player.playerEvents.AirDived.RemoveListener(OnAirDive);
        player.playerEvents.StompLanded.RemoveListener(OnStompLanding);
        player.playerEvents.DashStarted.RemoveListener(OnDash);
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        int index = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[index]);
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

    private void OnJumped()
    {
        PlayRandomClip(jump);
    }

    private void OnHurt()
    {
        PlayRandomClip(hurt);
    }

    private void OnAttacked()
    {
        PlayRandomClip(attack);
    }

    private void OnLifted()
    {
        PlayRandomClip(lift);
    }
    private void OnManeuver()
    {
        PlayRandomClip(maneuver);
    }

    private void OnSpun()
    {
        audioSource.PlayOneShot(spin);
    }

    private void OnPickUp()
    {
        audioSource.PlayOneShot(pickup);
    }
    private void OnDrop()
    {
        audioSource.PlayOneShot(drop);
    }
    private void OnAirDive()
    {
        audioSource.PlayOneShot(airDive);
    }
    private void OnStompSpin()
    {
        audioSource.PlayOneShot(stompSpin);
    }
    private void OnStompLanding()
    {
        audioSource.PlayOneShot(stompLanding);
    }
    private void OnLedgeGrabbing()
    {
        audioSource.PlayOneShot(ledgeGrabbing);
    }
    private void OnDash()
    {
        audioSource.PlayOneShot(dash);
    }
    private void OnStartRailGrind()
    {
        audioSource.PlayOneShot(startRailGrind);
    }
    private void OnRailGrind()
    {
        audioSource.PlayOneShot(railGrind);
    }
}