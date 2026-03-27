using UnityEngine;

public abstract class EntityStatsManager<T> : MonoBehaviour where T : EntityStats<T>
{
    [SerializeField] private T[] stats;
    public T Current { get; private set; }

    protected virtual void Awake()
    {
        if (stats.Length > 0)
        {
            Current = stats[0];
        }
    }

    public void ChangeTo(int index)
    {
        if (index < 0 || index >= stats.Length)
        {
            Debug.LogError($"Invalid stats index: {index}");
            return;
        }
        
        if (Current != stats[index]) {
            Current = stats[index];
        }
    }
}