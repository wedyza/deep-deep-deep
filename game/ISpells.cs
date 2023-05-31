using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace game;

public interface ISpell : ICloneable
{
    public enum MagicType
    {
        fire,
        ice,
        death,
        light,
        none
    }

    MagicType _magicType { get; set; }
    
    int DamageDeals { get; set; }
}