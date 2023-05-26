﻿using System;
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

        private int _tileSize = 128;
        private char[,] _map = new char[15, 9];
        
        
        public int _currentID;

        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }

        public enum ObjectTypes : byte
        {
            player,
            wall,
            door,
            fire,
            ice,
            arrow,
            fire_arrow,
            attack,
            long_attack, 
            goblin, 
            skeleton,
            gremyt
        }


        public void PlayerAttack(IGameplayModel.Direction dir)
        {
            Player p = (Player)Objects[PlayerId];
            var spell = p.ActiveSpell.Clone() as IObject;
            if (dir == IGameplayModel.Direction.right)
            {
                spell.Speed = new Vector2(35, 0);
                spell.Move(new Vector2(p.Pos.X + 128, p.Pos.Y + 32));
                
            }
            else if (dir == IGameplayModel.Direction.left)
            {
                spell.Speed = new Vector2(-35, 0);
                spell.Move(new Vector2(p.Pos.X - 64, p.Pos.Y + 32));
            }
            else if (dir == IGameplayModel.Direction.forward)
            {
                spell.Speed = new Vector2(0, -35);
                spell.Move(new Vector2(p.Pos.X + 32, p.Pos.Y - 64));
            }
            else if (dir == IGameplayModel.Direction.backward)
            {
                spell.Speed = new Vector2(0, 35);
                spell.Move(new Vector2(p.Pos.X+32, p.Pos.Y + 128));
            }
            spell.dir = dir;
            Objects.Add(_currentID, spell);
            _currentID++;
        }

        public void ResetGame()
        {
        }

        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();
            _currentID = 1;
            _map[5, 4] = 'P';
            _map[9, 4] = 'G';
            createWallsOnEdges();
            _map[7, 0] = 'D';
            bool isPlacedPlayer = false;
            for (int y = 0; y < _map.GetLength(1); y++)
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    if (_map[x, y] != '\0')
                    {
                        IObject generatedObject = GenerateObject(_map[x, y], x, y);
                        if (_map[x, y] == 'P' && !isPlacedPlayer)
                        {
                            PlayerId = _currentID;
                            isPlacedPlayer = true;
                        }

                        Objects.Add(_currentID, generatedObject);
                        _currentID++;
                    }
                }
        }

        private Goblin CreateGoblin(float x, float y, ObjectTypes spriteId)
        {
            Goblin obj = new Goblin(new Vector2(x, y));
            obj.ImageID = (byte)spriteId;
            return obj;
        }
        
        private Door CreateDoor(float x, float y, ObjectTypes spriteId)
        {
            Door obj = new Door(new Vector2(x, y));
            obj.ImageID = (byte)spriteId;
            return obj;
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
            Wall w = new Wall(new Vector2(x, y));
            w.ImageID = (byte)spriteId;
            return w;
        }

        private IObject GenerateObject(char sign, int xTile, int yTile)
        {
            float x = xTile * _tileSize;
            float y = yTile * _tileSize;
            IObject generatedObject = null;
            if (sign == 'O' || sign == 'P')
                generatedObject = CreateNPC(x, y, ObjectTypes.player, new Vector2(0, 0));
            else if (sign == 'W')
                generatedObject = CreateWall(x, y, ObjectTypes.wall);
            else if (sign == 'D')
                generatedObject = CreateDoor(x, y, ObjectTypes.door);
            else if (sign == 'G')
                generatedObject = CreateGoblin(x, y, ObjectTypes.goblin);
            return generatedObject;
        }

        public void createWallsOnEdges()
        {
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                _map[x, 0] = 'W';
                _map[x, _map.GetLength(1) - 1] = 'W';
            }

            for (int y = 1; y < _map.GetLength(1); y++)
            {
                _map[0, y] = 'W';
                _map[_map.GetLength(0) - 1, y] = 'W';
            }
        }

        public void Update()
        {
            for (int i = 1; i <= Objects.Keys.Count; i++)
            {
                var objInitPos = Objects[i].Pos;
                Objects[i].Update();
                if (Objects[i] is ISolid p1 && objInitPos != Objects[i].Pos)
                {
                    for (int j = 1; j <= Objects.Keys.Count; j++)
                    {
                        if (i == j)
                            continue;
                        if (Objects[j] is ISolid p2)
                        {
                            while (RectangleCollider.IsCollided(p1.Collider, p2.Collider))
                            {
                                if (Objects[i] is ISpell)
                                {
                                    Objects.Remove(i);
                                    _currentID--;
                                    break;
                                }
                                var oppositeDir = Objects[i].Pos - objInitPos;
                                oppositeDir.Normalize();
                                Objects[i].Move(Objects[i].Pos - oppositeDir);
                            }
                        }

                        if (!Objects.ContainsKey(i))
                            break;
                    }
                }
            }

            Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects });
        }

        public void MovePlayer(IGameplayModel.Direction dir)
        {
            Player p = (Player)Objects[PlayerId];
            switch (dir)
            {
                case IGameplayModel.Direction.forward:
                    p.Speed += new Vector2(0, -3);
                    break;
                case IGameplayModel.Direction.backward:
                    p.Speed += new Vector2(0, 3);
                    break;
                case IGameplayModel.Direction.left:
                    p.Speed += new Vector2(-3, 0);
                    break;
                case IGameplayModel.Direction.right:
                    p.Speed += new Vector2(3, 0);
                    break;
            }
        }

        private void CalculateObstacleCollision(
            (Vector2 initPos, int id) obj1,
            (Vector2 initPos, int id) obj2
        )
        {
            bool isCollided = false;
            if (Objects[obj1.id] is ISolid p1 && Objects[obj2.id] is ISolid p2)
            {
                Vector2 oppositeDir = new Vector2(0, 0);
                while (RectangleCollider.IsCollided(p1.Collider, p2.Collider))
                {
                    isCollided = true;
                    if (obj1.initPos != Objects[obj1.id].Pos)
                    {
                        oppositeDir = Objects[obj1.id].Pos - obj1.initPos;
                        oppositeDir.Normalize();
                        Objects[obj1.id].Move(Objects[obj1.id].Pos - oppositeDir);
                    }
                    if (obj2.initPos != Objects[obj2.id].Pos)
                    {
                        oppositeDir = Objects[obj2.id].Pos - obj2.initPos;
                        oppositeDir.Normalize();
                        Objects[obj2.id].Move(Objects[obj2.id].Pos - oppositeDir);
                    }
                }
            }

            if (isCollided)
            {
                Objects[obj1.id].Speed = new Vector2(0, 0);
                Objects[obj2.id].Speed = new Vector2(0, 0);
            }
        }
    }
}