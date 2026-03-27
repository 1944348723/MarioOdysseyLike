using System;
using UnityEngine.Events;

[Serializable]
public class EntityStateManagerEvents
{
    public UnityEvent<Type> Entered;
    public UnityEvent Changed;
    public UnityEvent<Type> Exited;
}