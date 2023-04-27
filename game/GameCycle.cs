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

        private int _tileSize = 100;
        private char[,] _map = new char[16, 9];

        public int _currentID;

        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }


        public enum ObjectTypes : byte
        {
            player,
            wall
        }


        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();
            _currentID = 1;
            _map[5, 6] = 'P';
            _map[9, 4] = 'O';
            _map[15, 6] = 'O';
            createWallsOnEdges();
            bool isPlacedPlayer = false;
            for (int y = 0; y < _map.GetLength(1); y++)
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    if (_map[x, y] != '\0')
                    {
                        IObject generatedObject = GenerateObject(_map[x, y], x, y);
                        if (_map[x,y] == 'P' && !isPlacedPlayer)
                        {
                            PlayerId = _currentID;
                            isPlacedPlayer = true;
                        }
                        Objects.Add(_currentID, generatedObject);
                        _currentID++;
                    }
                }
        }


        private Player CreateNPC(float x, float y, ObjectTypes spriteId, Vector2 speed)
        {
            Player obj = new Player(new Vector2(x, y));
            obj.ImageID = (byte)spriteId;
            obj.Speed = speed;
            return obj;
        }

        private Wall CreateWall(float x, float y, ObjectTypes spriteId)
        {
            Wall w = new Wall();
            w.ImageID = (byte)spriteId;
            w.Pos = new Vector2(x, y);
            return w;
        }

        private IObject GenerateObject(char sign, int xTile, int yTile)
        {
            float x = xTile * _tileSize;
            float y = yTile * _tileSize;
            IObject generatedObject = null;
            if (sign == 'O' || sign == 'P')
                generatedObject = CreateNPC(x + _tileSize / 2 - 38, y + _tileSize / 2 - 50, ObjectTypes.player, new Vector2(0, 0));
            else if (sign == 'W')
                generatedObject = CreateWall(x + _tileSize / 2 - 12, y + _tileSize / 2 - 50, ObjectTypes.wall);
            return generatedObject;
        }

        public void createWallsOnEdges()
        {
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                _map[x, 0] = 'W';
                _map[x, _map.GetLength(1)-1] = 'W';
            }

            for (int y = 1; y < _map.GetLength(1); y++)
            {
                _map[0, y] = 'W';
                _map[_map.GetLength(0)-1, y] = 'W';
            }
        }

        public void Update()
        {
            for (int i = 1; i <= Objects.Keys.Count; i++)
            {
                Objects[i].Update();
                if (Objects[i] is Player p1)
                {
                    for (int j = i+1; j <= Objects.Keys.Count; j++)
                    {
                        if (Objects[j] is ISolid p2)
                        {
                            if (RectangleCollider.IsCollided(p1.Collider, p2.Collider))
                            {
                                Objects[i].Speed = new Vector2(0, 0);
                                Objects[j].Speed = new Vector2(0, 0);
                            }
                        }
                    }
                }
            }
            Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects });
        }

        public void MovePlayer(IGameplayModel.Direction dir)
        {
            Console.WriteLine(PlayerId);
            Player p = (Player)Objects[PlayerId];
            Console.WriteLine(p);
            switch (dir)
            {
                case IGameplayModel.Direction.forward:
                    {
                        p.Speed += new Vector2(0, -1);
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