using System;
using UnityEngine;

public class Button3D : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private Vector3 pressedOffset = new(0, -0.1f, 0);
    [SerializeField] private float animationDuration = 0.1f;
    [SerializeField] private AudioClip activateClip;
    [SerializeField] private AudioClip deactivateClip;

    public event Action Pressed;
    public event Action Released;

    private AudioSource audioSource;
    private Vector3 initPosition;
    private bool isPressed = false;

    private void Awake()
    {
        if (activateClip || deactivateClip)
        {
            if (!TryGetComponent<AudioSource>(out audioSource))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }
    
    private void Start()
    {
        initPosition = button.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        Press();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        Release();
    }

    private void Press()
    {
        if (isPressed) return;

        isPressed = true;
        DoTween.To<Vector3>(
            () => button.transform.position,
            position => button.transform.position = position,
            initPosition + pressedOffset,
            animationDuration
        );
        if (activateClip)
        {
            audioSource.PlayOneShot(activateClip);
        }
        Pressed?.Invoke();
    }

    private void Release()
    {
        if (!isPressed) return;

        isPressed = false;
        DoTween.To<Vector3>(
            () => button.transform.position,
            position => button.transform.position = position,
            initPosition,
            animationDuration
        );
        if (deactivateClip)
        {
            audioSource.PlayOneShot(deactivateClip);
        }
        Released?.Invoke();
    }
}