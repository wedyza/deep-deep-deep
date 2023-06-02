namespace game;

public interface IArtefact
{
    float Multiplier { get; }
    public ISpell spell { get; }
    public string Description { get; }
    public ArtefactType Type { get; }
    enum ArtefactType
    {
        Spell,
        Characteristics
    }
}

public interface IHP
{
}

public interface IDMG
{
}

public interface ISpeed
{
}