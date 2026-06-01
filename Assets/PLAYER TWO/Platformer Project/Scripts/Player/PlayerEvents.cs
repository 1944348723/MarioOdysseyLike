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
    public UnityEvent StompEnded;
    public UnityEvent SpinStarted;
    public UnityEvent SpinEnded;
    public UnityEvent Dived;
    public UnityEvent GlideStarted;
    public UnityEvent GlideEnded;
    public UnityEvent WalkStarted;
    public UnityEvent WalkEnded;
}