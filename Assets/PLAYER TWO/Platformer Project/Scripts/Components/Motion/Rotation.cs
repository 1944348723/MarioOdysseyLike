using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private Vector3 axis = new(0, 1, 0);
    [SerializeField] private float speed = 30;

    private void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}