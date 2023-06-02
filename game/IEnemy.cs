namespace game;

public interface IEnemy
{
    public Player Target { get; set; }
    public int Damage { get; set; }
}