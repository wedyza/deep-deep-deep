using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace game
{
    public interface IObject
    {
        int ImageID { get; set; }

        Vector2 Pos { get; set; }

        Vector2 Speed { get; set; }

        void Update();
    }
}