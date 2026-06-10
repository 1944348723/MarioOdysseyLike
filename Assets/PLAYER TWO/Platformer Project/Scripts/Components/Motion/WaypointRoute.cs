using Unity.Mathematics;
using UnityEngine;

public class WaypointRoute : MonoBehaviour
{
    public enum Mode
    {
        Once,
        Loop,
        PingPong
    }

    [SerializeField] private Transform[] points;
    [SerializeField] private Mode mode;

    private int targetIndex = 0;
    private int step = 1;

    public int Count => points.Length;

    public Vector3 Get(int index)
    {
        return points[index].position;
    }

    public Vector3 Current => points[targetIndex].position;

    public void Advance()
    {
        if (points.Length <= 1) return;

        switch (mode)
        {
            case Mode.Once:
                targetIndex = math.min(targetIndex + step, points.Length - 1);
                break;
            case Mode.Loop:
                targetIndex = (targetIndex + 1) % points.Length;
                break;
            case Mode.PingPong:
                if (targetIndex == 0)
                {
                    step = 1;
                } else if (targetIndex == points.Length - 1)
                {
                    step = -1;
                }
                targetIndex += step;
                break;
        }
    }
}