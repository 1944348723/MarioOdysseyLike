using System;
using UnityEngine.Events;

[Serializable]
public class EntityEvents
{
    public UnityEvent EnterGround;
    public UnityEvent ExitGround;
    public UnityEvent EnterRails;
    public UnityEvent ExitRails;
}