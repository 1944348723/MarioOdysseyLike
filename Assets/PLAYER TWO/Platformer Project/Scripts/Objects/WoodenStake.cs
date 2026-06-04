using UnityEngine;

public class WoodenStake : MonoBehaviour, IEntityContact
{
    [SerializeField] private Collider stakeCollider;
    [SerializeField] private GameObject coin;
    [SerializeField] private float offset = 1.5f;
    [SerializeField] private float animationDuration = 0.2f;

    private void Awake()
    {
        if (!stakeCollider)
        {
            stakeCollider = GetComponent<Collider>();
        }
    }
    
    public void OnEntityContact(EntityBase entity)
    {
        if (ShouldActivate(entity))
        {
            Activate();
        }
    }

    private bool ShouldActivate(EntityBase entity)
    {
        if (entity is not Player) return false;

        Player player = entity as Player;
        return player.StateMachine.CurrentState is StompPlayerState
            && player.transform.position.y > stakeCollider.bounds.max.y;
    }

    private void Activate()
    {
        coin.SetActive(true);
        DoTween.To<Vector3>(
            () => transform.position,
            position => transform.position = position,
            transform.position + Vector3.down * offset,
            animationDuration
        );
    }
}