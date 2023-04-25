using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game
{
    public class RectangleCollider
    {
        public Rectangle Boundary { get; set; }
        public RectangleCollider(int x, int y, int width, int height)
        {
            Boundary = new Rectangle(x, y, width, height);
        }

        public static bool IsCollided(RectangleCollider r1, RectangleCollider r2)
        {
            return r1.Boundary.Intesects(r2.Boundary);
        }
    }

    public interface ISolid
    {
        RectangleCollider Collider { get; set; }
        void MoveCollider(Vector2 newPos);
    }
}