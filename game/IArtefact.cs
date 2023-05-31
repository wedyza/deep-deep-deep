namespace game;

public interface IArtefact
{
    public int Multiplier { get; set; }
    public ISpell spell { get; set; }
    
    public string Description { get; set; }
    public ArtefactType Type { get; set; }
    enum ArtefactType
    {
        Spell,
        Characteristics
    }
}