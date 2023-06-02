using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace game;

public interface ISpell : ICloneable
{
    public enum CastType
    {
        projectTile,
        pillar
    }
    
    public enum MagicType
    {
        fire,
        ice,
        death,
        light,
        none
    }

    CastType _castType { get; set; }
    
    MagicType _magicType { get; set; }
    
    int DamageDeals { get; set; }

    void DeleteSkill();
    void Die(Object source, System.Timers.ElapsedEventArgs e);
}