using Microsoft.Xna.Framework;

namespace game;

class BookOfNecromancer : IArtefact, ISolid, IObject
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

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
    public float Multiplier { get; set; }
    public ISpell spell { get; set; }
    public string Description { get; set; }
    public IArtefact.ArtefactType Type { get; set; }

    public BookOfNecromancer(Vector2 pos)
    {
        Pos = pos;
        ImageID = (byte)GameCycle.ObjectTypes.book;
        Description = "В этой книги содержатся древние знания некромантов";
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        spell = new Undead();
        Type = IArtefact.ArtefactType.Spell;
        UnderEffect = ISpell.MagicType.none;
        dir = IGameplayModel.Direction.right;
        IsRemoved = false;
    }

    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }
}

class BookOfPyromancer : IArtefact, ISolid, IObject
{
    public float Multiplier { get; set; }
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
        dir = IGameplayModel.Direction.right;
        UnderEffect = ISpell.MagicType.none;
        IsRemoved = false;
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

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
}

class BookOfIcycle : IArtefact, ISolid, IObject
{
    public float Multiplier { get; set; }
    public ISpell spell { get; set; }
    public string Description { get; set; }
    public IArtefact.ArtefactType Type { get; set; }
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }

    public int HP { get; set; }
    public int ImageID { get; set; }
    public float SpeedMultiply { get; set; }
    public double DamageMultiply { get; set; }
    public IGameplayModel.Direction dir { get; set; }
    public Vector2 Pos { get; }
    public Vector2 Speed { get; set; }

    public BookOfIcycle(Vector2 pos)
    {
        Pos = pos;
        ImageID = (byte)GameCycle.ObjectTypes.book;
        Type = IArtefact.ArtefactType.Spell;
        spell = new Ice();
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        Description = "Похоже, что в этой книге содержатся древние знания заклинателей холода.";
        dir = IGameplayModel.Direction.right;
        UnderEffect = ISpell.MagicType.none;
        IsRemoved = false;
    }
    
    public void Move(Vector2 pos)
    {
    }

    public void Update()
    {
    }

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
}

public class BookOfLighters : IArtefact, IObject, ISolid
{
    public float Multiplier { get; }
    public ISpell spell { get; }
    public string Description { get; }
    public IArtefact.ArtefactType Type { get; }
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

    public BookOfLighters(Vector2 Pos)
    {
        this.Pos = Pos;
        ImageID = (byte)GameCycle.ObjectTypes.book;
        Type = IArtefact.ArtefactType.Spell;
        spell = new Light();
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        Description = "Похоже, что в этой книге содержатся древние знания заклинателей воздуха";
        dir = IGameplayModel.Direction.right;
        UnderEffect = ISpell.MagicType.none;
        IsRemoved = false;
    }
    
    public void Update()
    {
    }

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }
}

public class DragonClaw : ISolid, IArtefact, IObject, IDMG
{
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }

    public float Multiplier { get; }
    public ISpell spell { get; }
    public string Description { get; }
    public IArtefact.ArtefactType Type { get; }
    public int HP { get; set; }
    public int ImageID { get; set; }
    public float SpeedMultiply { get; set; }
    public double DamageMultiply { get; set; }
    public IGameplayModel.Direction dir { get; set; }

    public DragonClaw(Vector2 pos)
    {
        Pos = pos;
        Type = IArtefact.ArtefactType.Characteristics;
        Multiplier = 1.15f;
        ImageID = (byte)GameCycle.ObjectTypes.claw;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        Description = "Похоже, что это коготь древенего дракона Гремута";
        dir = IGameplayModel.Direction.right;
        UnderEffect = ISpell.MagicType.none;
        IsRemoved = false;
    }
    public Vector2 Pos { get; }
    public Vector2 Speed { get; set; }
    public void Move(Vector2 pos)
    {
    }

    public void Update()
    {
    }

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
}

public class Taproot : ISolid, IObject, IArtefact, IHP
{
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }

    public Taproot(Vector2 pos)
    {
        Pos = pos;
        Type = IArtefact.ArtefactType.Characteristics;
        Multiplier = 1.2f;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        Description = "Похоже, что этот корень самого миража две тысячи семисот пятого!";
        dir = IGameplayModel.Direction.right;
        ImageID = (byte)GameCycle.ObjectTypes.taproot;
        UnderEffect = ISpell.MagicType.none;
        IsRemoved = false;
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

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
    public float Multiplier { get; }
    public ISpell spell { get; }
    public string Description { get; }
    public IArtefact.ArtefactType Type { get; }
}

public class Boots : IArtefact, ISpeed, IObject, ISolid
{
    public float Multiplier { get; }
    public ISpell spell { get; }
    public string Description { get; }
    public IArtefact.ArtefactType Type { get; }
    public int HP { get; set; }

    public Boots(Vector2 pos)
    {
        Pos = pos;
        ImageID = (byte)GameCycle.ObjectTypes.boots;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        Type = IArtefact.ArtefactType.Characteristics;
        Multiplier = 1.13f;
        Description = "Ботинки охотника с севера - Максима плоского";
        dir = IGameplayModel.Direction.right;
        UnderEffect = ISpell.MagicType.none;
        IsRemoved = false;
    }
    
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

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }
}

public class Cane : IHP, IDMG, ISpeed, ISolid, IObject, IArtefact
{
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }

    public Cane(Vector2 pos)
    {
        Pos = pos;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        ImageID = (byte)GameCycle.ObjectTypes.cane;
        Multiplier = 1.07f;
        Description = "Легендарная трость Герегорда..";
        Type = IArtefact.ArtefactType.Characteristics;
        dir = IGameplayModel.Direction.right;
        UnderEffect = ISpell.MagicType.none;
        IsRemoved = false;
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

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
    public float Multiplier { get; }
    public ISpell spell { get; }
    public string Description { get; }
    public IArtefact.ArtefactType Type { get; }
}