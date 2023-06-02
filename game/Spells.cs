using System;
using Microsoft.Xna.Framework;
using System.Timers;

namespace game;

public class Fire : ISpell, IObject, ISolid
{
    public ISpell.CastType _castType { get; set; }
    public ISpell.MagicType _magicType { get; set; }
    public int DamageDeals { get; set; }
    public void DeleteSkill()
    {
        throw new NotImplementedException();
    }

    public void Die(object source, ElapsedEventArgs e)
    {
        throw new NotImplementedException();
    }

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

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }

    public Fire()
    {
        _castType = ISpell.CastType.projectTile;
        UnderEffect = ISpell.MagicType.none;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        Enemy = false;
        _magicType = ISpell.MagicType.fire;
        ImageID = (byte)GameCycle.ObjectTypes.fire;
        DamageDeals = 150;
        IsRemoved = false;
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
    public ISpell.CastType _castType { get; set; }
    public ISpell.MagicType _magicType { get; set; }
    public int DamageDeals { get; set; }
    public void DeleteSkill()
    {
        throw new NotImplementedException();
    }

    public void Die(object source, ElapsedEventArgs e)
    {
        throw new NotImplementedException();
    }

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

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }


    public Ice()
    {
        _castType = ISpell.CastType.projectTile;
        UnderEffect = ISpell.MagicType.none;
        Enemy = false;
        _magicType = ISpell.MagicType.ice;
        ImageID = (byte)GameCycle.ObjectTypes.ice;
        DamageDeals = 100;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 64, 64);
        IsRemoved = false;
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

    public ISpell.CastType _castType { get; set; }
    public ISpell.MagicType _magicType { get; set; }
    public int DamageDeals { get; set; }
    public void DeleteSkill()
    {
        Timer aTimer = new System.Timers.Timer();
        aTimer.Interval = 150;
        aTimer.Elapsed += Die;
        aTimer.AutoReset = false;
        aTimer.Enabled = true;
    }

    public void Die(object source, ElapsedEventArgs e)
    {
        IsRemoved = true;
    }

    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }

    public Undead()
    {
        _castType = ISpell.CastType.pillar;
        DamageDeals = 150;
        ImageID = (byte)GameCycle.ObjectTypes.undead;
        _magicType = ISpell.MagicType.death;
        dir = IGameplayModel.Direction.right;
        UnderEffect = ISpell.MagicType.none;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 127, 127);
        IsRemoved = false;
    }
    
    public int HP { get; set; }
    public int ImageID { get; set; }
    public float SpeedMultiply { get; set; }
    public double DamageMultiply { get; set; }
    public IGameplayModel.Direction dir { get; set; }
    public Vector2 Pos { get; private set; }
    public Vector2 Speed { get; set; }
    public void Move(Vector2 pos)
    {
        Pos = pos;
    }

    public void Update()
    {
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 127, 127);
    }

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
}

public class Light : ISpell, ISolid, IObject
{
    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public ISpell.CastType _castType { get; set; }
    public ISpell.MagicType _magicType { get; set; }
    public int DamageDeals { get; set; }
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
    }

    public int HP { get; set; }
    public int ImageID { get; set; }
    public float SpeedMultiply { get; set; }
    public double DamageMultiply { get; set; }
    public IGameplayModel.Direction dir { get; set; }
    public Vector2 Pos { get; private set; }
    public Vector2 Speed { get; set; }
    public void Move(Vector2 pos)
    {
        Pos = pos;
    }

    public Light()
    {
        _castType = ISpell.CastType.pillar;
        DamageDeals = 100;
        _magicType = ISpell.MagicType.light;
        dir = IGameplayModel.Direction.right;
        ImageID = (byte)GameCycle.ObjectTypes.light;
        UnderEffect = ISpell.MagicType.none;
        IsRemoved = false;
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 127, 127);
    }
    
    public void Update()
    {
        Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 127, 127);
    }

    public void DeleteSkill()
    {
        Timer aTimer = new System.Timers.Timer();
        aTimer.Interval = 150;
        aTimer.Elapsed += Die;
        aTimer.AutoReset = false;
        aTimer.Enabled = true;
    }
    public void Die(Object source, System.Timers.ElapsedEventArgs e)
    {
        IsRemoved = true;
    }

    public bool IsRemoved { get; set; }

    public ISpell.MagicType UnderEffect { get; set; }
}