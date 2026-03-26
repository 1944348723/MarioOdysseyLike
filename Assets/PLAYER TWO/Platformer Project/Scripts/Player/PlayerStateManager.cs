using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerStateManager: EntityStateManager<Player>
{
    [ClassTypeName(typeof(PlayerState))]
    [SerializeField] private string[] states;

    protected override List<EntityState<Player>> GetStatesList()
    {
        return EntityState<Player>.CreateListFromStringArray(states);
    }
}