using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PoleDetector : MonoBehaviour
{
    [SerializeField] private Collider detectCollider;

    public Pole CurrentPole { get; private set; } = null;

    private void Awake()
    {
        if (!detectCollider)
        {
            detectCollider = GetComponent<Collider>();
        }
        detectCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Pole pole = other.GetComponentInParent<Pole>();
        if (pole)
        {
            Debug.Log("Pole Detected");
            CurrentPole = pole;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Pole pole = other.GetComponentInParent<Pole>();
        if (pole && pole == CurrentPole)
        {
            Debug.Log("Pole Lost");
            CurrentPole = null;
        }
    }
}