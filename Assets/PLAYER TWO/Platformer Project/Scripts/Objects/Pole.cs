using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField] private Collider poleCollider = null;
    public float MaxHeight => poleCollider.bounds.max.y;


    private void Awake()
    {
        if (!poleCollider)
        {
            poleCollider = GetComponent<Collider>();
        }
    }
}