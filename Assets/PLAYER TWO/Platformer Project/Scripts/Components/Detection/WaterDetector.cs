using System;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    public WaterVolume CurrentWater { get; private set; }
    public bool IsInWater => CurrentWater != null;
    public float DepthBelowSurface => IsInWater
        ? CurrentWater.SurfaceY - transform.position.y
        : 0;

    public event Action<WaterVolume> EnteredWater;
    public event Action<WaterVolume> ExitedWater;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<WaterVolume>(out WaterVolume water))
        {
            CurrentWater = water;
            EnteredWater?.Invoke(CurrentWater);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<WaterVolume>(out WaterVolume water) && water == CurrentWater)
        {
            ExitedWater?.Invoke(CurrentWater);
            CurrentWater = null;
        }
    }
}