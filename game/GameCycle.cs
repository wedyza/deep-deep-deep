using System;
using System.Collections.Generic;
using System.Diagnostics;
using deep_deep_deep;
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
        private Dictionary<(int, int), IRooms> MapComposition;
        private int XMap = 0;
        private int YMap = 0;
        private Dictionary<int, Vector2> collisionObjects;
        private Room ActualRoom;
        private Player Player;
        private int RoomCounter;
        public int _currentID;
        private IObject PlayerContainer;
        
        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }

        public enum ObjectTypes : byte
        {
            player,
            wall,
            door,
            fire,
            ice,
            undead,
            light,
            goblin, 
            skeleton,
            book
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

        public void ChangeSpell(ISpell spell)
        {
            var p = Objects[PlayerId] as Player;
            p.ActiveSpell = spell;
        }

        public void ResetGame()
        {
        }

        public void Initialize()
        {
            RoomCounter = 0;
            Player = new Player();
            MapComposition = new Dictionary<(int, int), IRooms>();
            ActualRoom = new Room(Room.RoomType.StartRoom, IGameplayModel.Direction.none, (true, true, true, true));
            MapComposition.Add((XMap, YMap), ActualRoom);
            ActualRoom.IsCleared = true;
            RoomInitialize(ActualRoom);
        }

        public void RoomInitialize(Room room)
        {
            _currentID = 0;
            bool isPlacedPlayer = false;
            room.PlacePlayer();
            
            Objects = new Dictionary<int, IObject>();
            for (int y = 0; y < room.Map.GetLength(1); y++)
            for (int x = 0; x < room.Map.GetLength(0); x++)
            {
                if (room.Map[x, y] != '\0')
                {
                    IObject generatedObject = GenerateObject(room.Map[x, y], x, y);
                    if (room.IsCleared && room.Map[x, y] == 'G')
                        break;
                    if (room.Map[x, y] == 'P' && !isPlacedPlayer)
                    {
                        PlayerId = _currentID;
                        isPlacedPlayer = true;
                        PlayerContainer = generatedObject;
                        _currentID++;
                        continue;
                    }
                    Objects.Add(_currentID, generatedObject);
                    _currentID++;
                }
            }
            Objects.Add(PlayerId, PlayerContainer);
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
            obj.DoorDirection = x == 0 ? IGameplayModel.Direction.left :
                y == 0 ? IGameplayModel.Direction.forward :
                y / _tileSize < 7 ? IGameplayModel.Direction.right :
                IGameplayModel.Direction.backward;
            obj.x = obj.DoorDirection == IGameplayModel.Direction.left ? -1 :
                obj.DoorDirection == IGameplayModel.Direction.right ? 1 : 0;
            obj.y = obj.DoorDirection == IGameplayModel.Direction.backward ? -1 :
                IGameplayModel.Direction.forward == obj.DoorDirection ? 1 : 0;
            return obj;
        }

        private Player CreateNPC(float x, float y, Vector2 speed)
        {
            var obj = Player;
            obj.Speed = speed;
            obj.Move(new Vector2(x, y));
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
            if (sign == 'P')
                generatedObject = CreateNPC(x, y, new Vector2(0, 0));
            else if (sign == 'W')
                generatedObject = CreateWall(x, y, ObjectTypes.wall);
            else if (sign == 'D')
                generatedObject = CreateDoor(x, y, ObjectTypes.door);
            else if (sign == 'G')
                generatedObject = CreateGoblin(x, y, ObjectTypes.goblin);
            return generatedObject;
        }

        public void Update()
        {
            collisionObjects = new Dictionary<int, Vector2>();
            foreach (var i in Objects.Keys)
            {
                var objInitPos = Objects[i].Pos;
                if (Objects[i] is IEnemy e) e.Target = Objects[PlayerId] as Player;
                Objects[i].Update();
                collisionObjects.Add(i, objInitPos);
            }
            
            foreach (var i in collisionObjects.Keys)
                foreach (var j in collisionObjects.Keys)
                {
                    if (i != j) 
                        CalculateObstacleCollision(
                            (collisionObjects[j], j),
                            (collisionObjects[i], i) // ЕСЛИ СКИЛЛЫ НЕ РАБОТАЮТ, ТО ПРОБЛЕМА ТУТ!!
                            );
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
            var rand = new Random();
            bool isCollided = false;
            if (Objects[obj1.id] is ISolid p1 && Objects[obj2.id] is ISolid p2)
            {
                Vector2 oppositeDir = new Vector2(0, 0);
                while (RectangleCollider.IsCollided(p1.Collider, p2.Collider))
                {
                    if (Objects[obj1.id] is ISpell s1)
                    {
                        if (Objects[obj2.id] is IEnemy)
                        {
                            Objects[obj2.id].UnderEffect = s1._magicType;
                            Objects[obj2.id].HP -= (int)(s1.DamageDeals * Objects[obj2.id].DamageMultiply);
                            if (Objects[obj2.id].HP <= 0)
                            {
                                Objects.Remove(obj2.id);
                                collisionObjects.Remove(obj2.id);
                            }
                        }
                        Objects.Remove(obj1.id);
                        collisionObjects.Remove(obj1.id);
                        break;
                    }
                    if (Objects[obj2.id] is Door d)
                    {
                        if (Objects[obj1.id] is Player)
                        {
                            if (ActualRoom.IsCleared)
                            {
                                XMap += d.x;
                                YMap += d.y;
                                if (MapComposition.ContainsKey((XMap, YMap)))
                                {
                                    ActualRoom = MapComposition[(XMap, YMap)] as Room;
                                    ActualRoom.dir = d.DoorDirection;
                                    RoomInitialize(ActualRoom);
                                }
                                else
                                {
                                    var left = CalculateSides(-1, 0);
                                    var right = CalculateSides(1, 0);
                                    var forward = CalculateSides(0, 1);
                                    var backward = CalculateSides(0, -1);
                                    var roomType = 
                                    ActualRoom = new Room(Room.RoomType.Killing, d.DoorDirection, (left, forward, right, backward));
                                    ActualRoom.IsCleared = true;
                                    MapComposition.Add((XMap, YMap), ActualRoom);
                                    RoomInitialize(ActualRoom);
                                }
                                break;
                            }
                        }
                    }
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
            if (Objects.ContainsKey(obj1.id) && Objects.ContainsKey(obj2.id))
                if (isCollided)
                {
                    Objects[obj1.id].Speed = new Vector2(0, 0);
                    Objects[obj2.id].Speed = new Vector2(0, 0);
                }
        }

        public bool CalculateSides(int d1, int d2)
        {
            var rand = new Random();
            bool side;
            if (MapComposition.ContainsKey((XMap + d1, YMap + d2)))
                side = MapComposition[(XMap + d1, YMap + d2)] is Room;
            else
            {
                side  = rand.Next(2) == 1;
                if (!side)
                    MapComposition.Add((XMap + d1, YMap + d2), new Nothing());
            }

            return side;
        }

        public Room.RoomType GenerateRoomType()
        {
            var rand = new Random();
            var types = new Room.RoomType[] { Room.RoomType.Killing, Room.RoomType.Looting };
        }
    }
}