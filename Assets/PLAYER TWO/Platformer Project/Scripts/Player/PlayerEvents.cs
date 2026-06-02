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
    public UnityEvent StompStarted;
    public UnityEvent StompLanded;
    public UnityEvent StompEnded;
    public UnityEvent SpinStarted;
    public UnityEvent SpinEnded;
    public UnityEvent AirDived;
    public UnityEvent GlideStarted;
    public UnityEvent GlideEnded;
    public UnityEvent WalkStarted;
    public UnityEvent WalkEnded;
}