
using UnityEngine;

public class DiePlayerState : PlayerState
{
    protected override void OnEnter(Player player)
    {
        Debug.Log("DiePlayerState Entered");
    }

    protected override void OnExit(Player player) {
        Debug.Log("DiePlayerState Exited");
    }

    protected override void OnStep(Player player) {
        player.Gravity();
        player.SnapToGround();
    }
}