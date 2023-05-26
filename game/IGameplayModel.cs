using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace game
{
    public interface IGameplayModel
    {
        event EventHandler<GameplayEventArgs> Updated;
        int PlayerId { get; set; }
        Dictionary<int, IObject> Objects { get; set; }  

        void Update();
        void MovePlayer(Direction dir);
        void PlayerAttack(Direction dir);
        void ResetGame();
        void Initialize();

        public enum Direction : byte
        {
            forward,
            backward,
            right,
            left
        }
    }


    public class GameplayEventArgs : EventArgs
    {
        public Dictionary<int, IObject> Objects { get; set; }
    }
}