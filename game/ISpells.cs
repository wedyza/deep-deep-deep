using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace game;

public interface ISpell : ICloneable
{
    public enum DamageType
    {
        physical,
        magical
    }

    DamageType _damageType { get; set; }
    
    int DamageDeals { get; set; }

    T SpecialEffect<T>(T x);
}