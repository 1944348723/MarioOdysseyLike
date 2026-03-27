using UnityEngine;

public class IdlePlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnExit(Player player)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnStep(Player player)
    {
        Debug.Log(player.Input.GetMovementDirection());
    }
}