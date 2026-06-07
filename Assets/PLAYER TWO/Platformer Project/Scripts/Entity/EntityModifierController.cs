using System.Collections.Generic;
using UnityEngine;

public class EntityModifierController : MonoBehaviour
{
    private readonly List<EntityModifier> modifiers = new();

    public void Add(EntityModifier modifier)
    {
        if (!modifiers.Contains(modifier))
        {
            modifiers.Add(modifier);
        }
    }

    public void Remove(EntityModifier modifier)
    {
        modifiers.Remove(modifier);
    }

    public float VelocityMultiplier
    {
        get
        {
            float value = 1f;
            foreach (var modifier in modifiers)
            {
                value *= modifier.velocityMultiplier;
            }
            return value;
        }
    }

    public float AccelerationMultiplier
    {
        get
        {
            float value = 1f;
            foreach (var modifier in modifiers)
            {
                value *= modifier.accelerationMultiplier;
            }
            return value;
        }
    }

    public float MaxSpeedMultiplier
    {
        get
        {
            float value = 1f;
            foreach (var modifier in modifiers)
            {
                value *= modifier.maxSpeedMultiplier;
            }
            return value;
        }
    }

    public float DecelerationMultiplier
    {
        get
        {
            float value = 1f;
            foreach (var modifier in modifiers)
            {
                value *= modifier.decelerationMultiplier;
            }
            return value;
        }
    }

    public float TurningDragMultiplier
    {
        get
        {
            float value = 1f;
            foreach (var modifier in modifiers)
            {
                value *= modifier.turningDragMultiplier;
            }
            return value;
        }
    }

    public float GravityMultiplier
    {
        get
        {
            float value = 1f;
            foreach (var modifier in modifiers)
            {
                value *= modifier.gravityMultiplier;
            }
            return value;
        }
    }
}