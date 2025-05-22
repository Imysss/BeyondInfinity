public class PlayerManager : Singleton<PlayerManager>
{
    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
}
