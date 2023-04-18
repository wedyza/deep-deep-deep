using System;
using System.Collections.Generic;
using game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated = delegate { };

        public int _currentID;

        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }

        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();
            _currentID = 1;
            Player player  = new Player();
            player.Pos = new Vector2(0, 0);
            player.ImageID = 1;
            player.Speed = new Vector2(0, 0);
            Objects.Add(_currentID, player);
            PlayerId = _currentID;
            _currentID++;
        }

        public void Update()
        {
            foreach(var obj in Objects.Values)
            {
                obj.Update();
            }
            Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects });
        }

        public void MovePlayer(IGameplayModel.Direction dir)
        {
            Player p = (Player)Objects[PlayerId];
            switch (dir)
            {
                case IGameplayModel.Direction.forward:
                    {
                        p.Speed+= new Vector2(0, -1);
                        break;
                    }
                case IGameplayModel.Direction.backward:
                    {
                        p.Speed += new Vector2(0, 1);
                        break;
                    }
                case IGameplayModel.Direction.left:
                    {
                        p.Speed += new Vector2(-1, 0);
                        break;
                    }
                case IGameplayModel.Direction.right:
                    {
                        p.Speed += new Vector2(1, 0);
                        break;
                    }

            }
        }
    }
}