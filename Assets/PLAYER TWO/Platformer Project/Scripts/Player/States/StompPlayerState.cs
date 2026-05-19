using UnityEngine;

public class StompPlayerState : PlayerState
{
    private enum StompState { HOVER, FALL, RECOVER }
    private StompState state;
    private float hoverTimer = 0f;
    private float recoverTimer = 0f;

    protected override void OnEnter(Player player)
    {
        state = StompState.HOVER;
        hoverTimer = 0f;
        recoverTimer = 0f;
        player.Velocity = Vector3.zero;
        player.playerEvents.StompStarted?.Invoke();
    }

    protected override void OnExit(Player player)
    {
        player.playerEvents.StompEnded?.Invoke();
    }

    protected override void OnStep(Player player)
    {
        if (player.TrySwim()) return;
        
        switch (state)
        {
            case StompState.HOVER: 
                if (hoverTimer >= player.Stats.Current.stompHoverDuration)
                {
                    player.VerticalVelocity = new(0, -player.Stats.Current.stompDownSpeed, 0);
                    state = StompState.FALL;
                }
                hoverTimer += Time.deltaTime;
                break;
            case StompState.FALL:
                if (player.IsGrounded)
                {
                    state = StompState.RECOVER;
                }
                break;
            case StompState.RECOVER:
                if (recoverTimer >= player.Stats.Current.stompRecoveryDuration)
                {
                    player.VerticalVelocity = new(0, player.Stats.Current.stompBounceSpeed, 0);
                    player.StateMachine.Change<FallPlayerState>();
                }
                recoverTimer += Time.deltaTime;
                break;
        }
    }
}