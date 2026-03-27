public class Player : Entity<Player>
{
    public PlayerInputSystem Input { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Input = GetComponent<PlayerInputSystem>();
    }
}