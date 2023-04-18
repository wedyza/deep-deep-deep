using Microsoft.Xna.Framework;
using System;

namespace game
{
    class Player : IObject
    {
        public int ImageID { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Speed { get; set; }

        public void Update()
        {
            Pos += Speed;
        }
    }
}
