using UnityEngine;

public class Spring : MonoBehaviour, IEntityContact
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private AudioClip clip;

    private AudioSource audioSource;

    private void OnValidate()
    {
        if (speed < 0) speed = 0;
    }

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnEntityContact(EntityBase entity)
    {
        if (entity is Player)
        {
            (entity as Player).VerticalVelocity = speed * Vector3.up;
            if (clip)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}