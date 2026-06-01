using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Serializable]
    private class Surface
    {
        public string tag;
        public AudioClip[] footsteps;
        public AudioClip[] landings;
    }

    [SerializeField] private Player player;
    [SerializeField] private float volume = 0.3f;
    [SerializeField] private float footstepOffset = 1.25f;
    [SerializeField] private Surface[] surfaces;
    [SerializeField] private AudioClip[] defaultFootsteps;
    [SerializeField] private AudioClip[] defaultLandings;

    private AudioSource audioSource;
    private Dictionary<String, AudioClip[]> footsteps = new();
    private Dictionary<String, AudioClip[]> landings = new();
    private Vector3 lastPlanarPosition;

    private void Awake()
    {
        if (!player)
        {
            player = FindAnyObjectByType<Player>();
        }

        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        foreach (Surface surface in surfaces)
        {
            footsteps.Add(surface.tag, surface.footsteps);
            landings.Add(surface.tag, surface.landings);
        }
        lastPlanarPosition = player.transform.position;
    }

    private void OnEnable()
    {
        player.entityEvents.EnterGround.AddListener(OnLanding);
    }

    private void OnDisable()
    {
        player.entityEvents.EnterGround.RemoveListener(OnLanding);
    }

    private void Update()
    {
        if (player.StateMachine.CurrentState is not WalkPlayerState || !player.IsGrounded) return;

        Vector3 planarPosition = player.transform.position;
        planarPosition.y = 0;
        float distance = (planarPosition - lastPlanarPosition).magnitude;
        if (distance > footstepOffset)
        {
            lastPlanarPosition = planarPosition;
            PlayFootstepClip(player.GroundHit.collider.tag);
        }
    }

    private void OnLanding()
    {
        if (player.PlayerWaterDetector.IsInWater) return;

        if (landings.ContainsKey(player.GroundHit.collider.tag))
        {
            PlayRandomClip(landings[player.GroundHit.collider.tag]);
        } else
        {
            PlayRandomClip(defaultLandings);
        }
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        int index = UnityEngine.Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[index], volume);
    }

    private void PlayFootstepClip(string tag)
    {
        if (footsteps.ContainsKey(tag))
        {
            PlayRandomClip(footsteps[tag]);
        } else
        {
            PlayRandomClip(defaultFootsteps);
        }
    }
}