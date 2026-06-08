using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 DeltaPosition { get; private set; }

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        DeltaPosition = transform.position - lastPosition;
        lastPosition = transform.position;
    }
}