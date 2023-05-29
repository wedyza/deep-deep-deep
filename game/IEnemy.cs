namespace game;

public interface IEnemy
{
    public Player Target { get; set; }
    public int Damage { get; set; }
    public double XSpeed { get; set; }
    public double YSpeed { get; set; }
    //public Artefact Artefact { get; set; } на будущее, артефакт, который дропается со врага
}