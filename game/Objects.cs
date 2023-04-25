using Microsoft.Xna.Framework;
using System;

namespace game
{
    class Player : IObject, ISolid
    {
        public int ImageID { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set; }

        public Car(Vector2 position)
        {
            Pos = position;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 200, 200);
        }

        public void Update()
        {
            Pos += Speed;
        }

        public void MoveCollider(Vector2 newPos)
        {
            Collider.Boundary.Offset(newPos);
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
