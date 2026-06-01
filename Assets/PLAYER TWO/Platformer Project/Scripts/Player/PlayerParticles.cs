using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem landingParticle;
    [SerializeField] private ParticleSystem dashParticle;
    [SerializeField] private ParticleSystem walkParticle;

    private void Awake()
    {
        if (!player)
        {
            player = GetComponentInParent<Player>();
        }
    }

    private void OnEnable()
    {
        player.entityEvents.EnterGround.AddListener(OnEnterGround);
        player.playerEvents.DashStarted.AddListener(OnDash);
        player.playerEvents.WalkStarted.AddListener(OnWalkStart);
        player.playerEvents.WalkEnded.AddListener(OnWalkEnd);
    }

    private void OnDisable()
    {
        player.entityEvents.EnterGround.RemoveListener(OnEnterGround);
        player.playerEvents.DashStarted.RemoveListener(OnDash);
        player.playerEvents.WalkStarted.RemoveListener(OnWalkStart);
        player.playerEvents.WalkEnded.RemoveListener(OnWalkEnd);
    }

    private void OnEnterGround()
    {
        if (!player.PlayerWaterDetector.IsInWater)
        {
            landingParticle.Play();
        }
    }

    private void OnDash()
    {
        dashParticle.Play();
    }

    private void OnWalkStart()
    {
        walkParticle.Play();
    }

    private void OnWalkEnd()
    {
        walkParticle.Stop();
    }
}