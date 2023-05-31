using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace game
{
    public interface IObject
    {
        int HP { get; set; }
        
        int ImageID { get; set; }
        float SpeedMultiply { get; set; }
        double DamageMultiply { get; set; }

        IGameplayModel.Direction dir { get; set; }

        Vector2 Pos { get; }

        Vector2 Speed { get; set; }

        void Move(Vector2 pos);

        void Update();
        
        ISpell.MagicType UnderEffect { get; set; }
    }
}