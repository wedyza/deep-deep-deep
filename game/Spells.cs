using System;
using Microsoft.Xna.Framework;

namespace game;

public class Fire : ISpell, IObject, ISolid
{
    public ISpell.MagicType _magicType { get; set; }
    public int DamageDeals { get; set; }
    
    public T SpecialEffect<T>(T x)
    {
        return x;
    }

    public int HP { get; set; }
    public int ImageID { get; set; }
    public float SpeedMultiply { get; set; }
    public double DamageMultiply { get; set; }
    public IGameplayModel.Direction dir { get; set; }
    public Vector2 Pos { get; private set; }
    public bool Enemy { get; }
    public Vector2 Speed { get; set; }
    public void Move(Vector2 pos)
    {
        Pos = pos;
        MoveCollider(Pos);
    }

    public void Update()
    {
        Pos += Speed;
        MoveCollider(Pos);
    }

    public ISpell.MagicType UnderEffect { get; set; }

    public Fire()
    {
        UnderEffect = ISpell.MagicType.none;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        Enemy = false;
        _magicType = ISpell.MagicType.fire;
        ImageID = (byte)GameCycle.ObjectTypes.fire;
        DamageDeals = 150;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
        Collider = new RectangleCollider((int)newPos.X, (int)newPos.Y, 64, 64);
    }
}

public class Ice : ISpell, ISolid, IObject
{
    public ISpell.MagicType _magicType { get; set; }
    public int DamageDeals { get; set; }

    public T SpecialEffect<T>(T x)
    {
        return x;
    }

    public int HP { get; set; }
    public int ImageID { get; set; }
    public float SpeedMultiply { get; set; }
    public double DamageMultiply { get; set; }
    public IGameplayModel.Direction dir { get; set; }
    public Vector2 Pos { get; private set; }
    public bool Enemy { get; }
    public Vector2 Speed { get; set; }
    public void Move(Vector2 pos)
    {
        Pos = pos;
        MoveCollider(Pos);
    }

    public void Update()
    {
        Pos += Speed;
        MoveCollider(Pos);
    }

    public ISpell.MagicType UnderEffect { get; set; }


    public Ice()
    {
        UnderEffect = ISpell.MagicType.none;
        Enemy = false;
        _magicType = ISpell.MagicType.ice;
        ImageID = (byte)GameCycle.ObjectTypes.ice;
        DamageDeals = 100;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
        Collider = new RectangleCollider((int)newPos.X, (int)newPos.Y, 64, 64);
    }
}

public class Undead : ISpell, ISolid, IObject
{
    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public ISpell.MagicType _magicType { get; set; }
    public int DamageDeals { get; set; }
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }

    public ISpell.MagicType UnderEffect { get; set; }
}