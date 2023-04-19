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
            Speed = new Vector2(0, Speed.Y);    
        }
    }

    class Wall : IObject
    {
        public int ImageID { get; set;}
        public Vector2 Pos { get; set; }

        public void Update()
        {

        }
    }
}
