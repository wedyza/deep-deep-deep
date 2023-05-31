using Microsoft.Xna.Framework;

namespace game;

class BookOfUndead : IArtefact, ISolid, IObject
{
    public int HP { get; set; }
    public int ImageID { get; set; }
    public float SpeedMultiply { get; set; }
    public double DamageMultiply { get; set; }
    public IGameplayModel.Direction dir { get; set; }
    public Vector2 Pos { get; }
    public Vector2 Speed { get; set; }
    public void Move(Vector2 pos)
    {
    }

    public void Update()
    {
    }

    public ISpell.MagicType UnderEffect { get; set; }
    public int Multiplier { get; set; }
    public ISpell spell { get; set; }
    public string Description { get; set; }
    public IArtefact.ArtefactType Type { get; set; }

    public BookOfUndead(Vector2 pos)
    {
        Pos = pos;
        ImageID = (byte)GameCycle.ObjectTypes.book;
        Description = "В этой книги содержатся древние знания некромантов";
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        spell = new Undead();
        Type = IArtefact.ArtefactType.Spell;
    }

    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }
}

class BookOfPyromancer : IArtefact, ISolid, IObject
{
    public int Multiplier { get; set; }
    public ISpell spell { get; set; }
    public string Description { get; set; }
    public IArtefact.ArtefactType Type { get; set; }
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }

    public BookOfPyromancer(Vector2 pos)
    {
        Pos = pos;
        ImageID = (byte)GameCycle.ObjectTypes.book;
        Description = "Похоже, что в этой книге содержатся древние знания пироманов.";
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        spell = new Fire();
        Type = IArtefact.ArtefactType.Spell;
    }

    public int HP { get; set; }
    public int ImageID { get; set; }
    public float SpeedMultiply { get; set; }
    public double DamageMultiply { get; set; }
    public IGameplayModel.Direction dir { get; set; }
    public Vector2 Pos { get; }
    public Vector2 Speed { get; set; }
    public void Move(Vector2 pos)
    {
    }

    public void Update()
    {
    }

    public ISpell.MagicType UnderEffect { get; set; }
}