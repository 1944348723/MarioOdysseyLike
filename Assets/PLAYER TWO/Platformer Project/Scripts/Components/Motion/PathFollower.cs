using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public enum MoveType
    {
        Transform,
        Rigidbody
    }

    [SerializeField] private WaypointRoute route;
    [SerializeField] private float speed = 1;
    [SerializeField] private MoveType moveType;

    private void OnValidate()
    {
        if (speed < 0) speed = 1;
    }

    private void Awake()
    {
        if (!route)
        {
            route = GetComponent<WaypointRoute>();
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 target = route.Current();
        if (transform.position == target)
        {
            route.Advance();
            target = route.Current();
        }
        
        // TODO: 支持其他移动方式
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
    }
}