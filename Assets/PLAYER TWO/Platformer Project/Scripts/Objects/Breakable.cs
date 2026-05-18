using UnityEngine;

[RequireComponent(typeof(Health))]
public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject breakEffectPrefab;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private GameObject dropItem;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.Died += OnBreak;
    }

    private void OnDestroy()
    {
        health.Died -= OnBreak;
    }

    private void OnBreak()
    {
        ShowBreakEffect();
        PlaySound();
        SpawnDropItem();
        Destroy(gameObject);
    }

    private void ShowBreakEffect()
    {
        if (breakEffectPrefab)
        {
            Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void PlaySound()
    {
       if (breakSound)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        } 
    }

    private void SpawnDropItem()
    {
        if (dropItem)
        {
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
    }
}