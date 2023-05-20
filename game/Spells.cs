using System;
using Microsoft.Xna.Framework;

namespace game;

public class Fire : ISpell, IObject, ISolid
{
    public ISpell.DamageType _damageType { get; set; }
    public int DamageDeals { get; set; }
    
    public T SpecialEffect<T>(T x)
    {
        return x;
    }
    public int ImageID { get; set; }
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

    public bool IsRemoved { get; set; }

    public Fire()
    {
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        Enemy = false;
        IsRemoved = false;
        _damageType = ISpell.DamageType.magical;
        ImageID = (byte)GameCycle.ObjectTypes.fire;
        DamageDeals = 75;
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
    public ISpell.DamageType _damageType { get; set; }
    public int DamageDeals { get; set; }

    public T SpecialEffect<T>(T x)
    {
        return x;
    }
    public int ImageID { get; set; }
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

    public bool IsRemoved { get; set; }

    public Ice()
    {
        IsRemoved = false;
        Enemy = false;
        _damageType = ISpell.DamageType.magical;
        ImageID = (byte)GameCycle.ObjectTypes.ice;
        DamageDeals = 50;
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