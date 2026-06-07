using UnityEngine;

public class SwampVolume : MonoBehaviour
{
    [SerializeField] private EntityModifier modifier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EntityBase entity))
        {
            entity.ModifierController.Add(modifier);
            entity.Velocity *= modifier.velocityMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EntityBase entity))
        {
            entity.ModifierController.Remove(modifier);
        }
    }
}