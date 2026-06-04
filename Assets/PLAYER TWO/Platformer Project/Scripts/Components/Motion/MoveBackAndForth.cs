using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    [SerializeField] private Vector3 offset1 = new(0, 1, 0);
    [SerializeField] private Vector3 offset2 = new(0, -1, 0);
    [SerializeField] private float speed = 1;

    private bool movingTowardTarget1 = true;
    private Vector3 target1;
    private Vector3 target2;

    private void OnValidate()
    {
        if (speed < 0)
        {
            speed = 1;
        }
    }

    private void Start()
    {
        target1 = transform.position + offset1;
        target2 = transform.position + offset2;
    }

    private void Update()
    {
        Vector3 target = movingTowardTarget1 ? target1 : target2;
        Vector3 currentPositionToTarget = target - transform.position;
        float distance = currentPositionToTarget.magnitude;
        if (speed * Time.deltaTime >= distance)
        {
            transform.position = target;
            movingTowardTarget1 = !movingTowardTarget1;
        } else
        {
            transform.position += currentPositionToTarget.normalized * speed * Time.deltaTime;
        }
    }
}