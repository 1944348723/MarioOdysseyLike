using System;
using UnityEngine.Events;

[Serializable]
public class PlayerEvents
{
    public UnityEvent Jumped;
    public UnityEvent Hurt;
    public UnityEvent Backfliped;
    public UnityEvent DashStarted;
    public UnityEvent DashEnded;
}