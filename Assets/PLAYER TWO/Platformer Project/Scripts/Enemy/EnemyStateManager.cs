using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStateManager: EntityStateManager<Enemy>
{
    [ClassTypeName(typeof(EnemyState))]
    [SerializeField] private string[] states;

    protected override List<EntityState<Enemy>> GetStatesList()
    {
        return EntityState<Enemy>.CreateListFromStringArray(states);
    }
}