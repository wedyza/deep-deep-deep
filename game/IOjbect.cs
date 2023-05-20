using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace game
{
    public interface IObject
    {
        int ImageID { get; set; }

        IGameplayModel.Direction dir { get; set; }

        Vector2 Pos { get; }
        
        bool Enemy { get; }

        Vector2 Speed { get; set; }

        void Move(Vector2 pos);

        void Update();
        
        bool IsRemoved { get; set; }
    }
}