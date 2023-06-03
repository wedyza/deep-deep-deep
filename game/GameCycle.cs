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
        private int _currentID;
        private IObject PlayerContainer;
        private int BossEncounter;
        private int LootingEncounter;

        public int PlayerId { get;  set; }
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
            boss,
            book,
            claw,
            cane,
            boots,
            taproot,
            tutorial
        }


        public void PlayerAttack(IGameplayModel.Direction dir)
        {
            var p = (Player)Objects[PlayerId];
            var spell = p.ActiveSpell.Clone() as IObject;
            if (p.ActiveSpell._castType == ISpell.CastType.projectTile)
            {
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
            }
            else
            {
                if (dir == IGameplayModel.Direction.right)
                    spell.Move(new Vector2(p.Pos.X + 128 + 256, p.Pos.Y));
                else if (dir == IGameplayModel.Direction.left)
                    spell.Move(new Vector2(p.Pos.X - 128 - 256, p.Pos.Y));
                else if (dir == IGameplayModel.Direction.forward)
                    spell.Move(new Vector2(p.Pos.X, p.Pos.Y  - 256));
                else if (dir == IGameplayModel.Direction.backward)
                    spell.Move(new Vector2(p.Pos.X, p.Pos.Y + 256));
                spell.dir = IGameplayModel.Direction.right;
                var real = spell as ISpell;
                real.DeleteSkill();
            }
            Objects.Add(_currentID, spell);
            _currentID++;
        }

        public void ChangeSpell(int id)
        {
            var p = Objects[PlayerId] as Player;
            if (p.Spells.Count - id > 0)
                p.ActiveSpell = p.Spells[id];
        }

        public void ResetGame()
        {
        }

        public void Initialize()
        {
            XMap = 0;
            YMap = 0;
            RoomCounter = 0;
            BossEncounter = 0;
            LootingEncounter = 0;
            collisionObjects = new Dictionary<int, Vector2>();
            _currentID = 0;
            Player = new Player();
            MapComposition = new Dictionary<(int, int), IRooms>();
            ActualRoom = new Room(Room.RoomType.StartRoom, IGameplayModel.Direction.none, (true, true, true, true));
            MapComposition.Add((XMap, YMap), ActualRoom);
            RoomInitialize(ActualRoom);
        }

        private void RoomInitialize(Room room)
        {
            _currentID = 0;
            bool isPlacedPlayer = false;
            room.PlacePlayer();
            
            Objects = new Dictionary<int, IObject>();
            collisionObjects = new Dictionary<int, Vector2>();
            for (int y = 0; y < room.Map.GetLength(1); y++)
            for (int x = 0; x < room.Map.GetLength(0); x++)
            {
                if (room.Map[x, y] != '\0')
                {
                    IObject generatedObject = GenerateObject(room.Map[x, y], x, y);
                    if (room.EnemiesInside <= 0 && (room.Map[x, y] == 'A' || room.Map[x, y] == 'G' || room.Map[x,y] == 'B'))
                        continue;
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

        private Boss CreateBoss(float x, float y)
        {
            Boss b = new Boss(new Vector2(x, y));
            b.HP = (int)(b.HP * (1 + MapComposition.Count * .02));
            return b;
        }
        
        private IObject CreateArtefact(float x, float y)
        {
            var rand = new Random();
            var num = rand.Next(4);
            var vector = new Vector2(x, y);
            IObject art = new Boots(vector);
            switch (num)
            {
                case 0:
                    art = new DragonClaw(vector);
                    break;
                case 1:
                    art = new Boots(vector);
                    break;
                case 2:
                    art = new Cane(vector);
                    break;
                case 3:
                    art = new Taproot(vector);
                    break;
            }
            return art;
        }
        
        private Goblin CreateGoblin(float x, float y, ObjectTypes spriteId)
        {
            Goblin obj = new Goblin(new Vector2(x, y));
            obj.ImageID = (byte)spriteId;
            obj.HP = (int)(obj.HP * (1 + MapComposition.Count * .02));
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
            else if (sign == 'A')
                generatedObject = CreateArtefact(x, y);
            else if (sign == 'B')
                generatedObject = CreateBoss(x, y);
            else if (sign == 'T')
                generatedObject = new Tutorial(new Vector2(x, y));
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
                if (Objects[i].IsRemoved)
                {
                    if (Objects[i] is Player)
                    {
                        Initialize();
                        break;
                    }
                    Objects.Remove(i);
                    continue;
                }
                collisionObjects.Add(i, objInitPos);
            }
            
            foreach (var i in collisionObjects.Keys)
                foreach (var j in collisionObjects.Keys)
                {
                    if (i != j) 
                        CalculateObstacleCollision(
                            (collisionObjects[j], j),
                            (collisionObjects[i], i) 
                            );
                }
            Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects, Rooms = this.MapComposition});
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
            if (Objects[obj1.id] is IEnemy && Objects[obj2.id] is IEnemy)
                return;
            bool isCollided = false;
            if (Objects[obj1.id] is ISolid p1 && Objects[obj2.id] is ISolid p2)
            {
                while (RectangleCollider.IsCollided(p1.Collider, p2.Collider))
                {
                    if (Objects[obj1.id] is ISpell s1)
                    {
                        if (Objects[obj2.id] is IEnemy)
                        {
                            Objects[obj2.id].UnderEffect = s1._magicType;
                            Objects[obj2.id].HP -= (int)(s1.DamageDeals * Objects[obj2.id].DamageMultiply * Objects[PlayerId].DamageMultiply);
                            if (Objects[obj2.id].HP <= 0)
                            {
                                if (Objects[obj2.id] is Boss b)
                                {
                                    Objects.Add(_currentID++, b.Artefact as IObject);
                                }
                                Objects[obj2.id].IsRemoved = true;
                                ActualRoom.EnemiesInside--;
                            }
                        }
                        Objects[obj1.id].IsRemoved = true;
                        collisionObjects.Remove(obj1.id);
                        break;
                    }
                    if (Objects[obj2.id] is Door d)
                    {
                        if (Objects[obj1.id] is Player)
                        {
                            if (ActualRoom.EnemiesInside <= 0)
                            {
                                XMap += d.x;
                                YMap += d.y;
                                if (MapComposition.ContainsKey((XMap, YMap)))
                                {
                                    ActualRoom.PlayerInside = false;
                                    ActualRoom = MapComposition[(XMap, YMap)] as Room;
                                    ActualRoom.PlayerInside = true;
                                    ActualRoom.dir = d.DoorDirection;
                                    RoomInitialize(ActualRoom);
                                }
                                else
                                {
                                    Room.RoomType roomType;
                                    var left = CalculateSides(-1, 0);
                                    var right = CalculateSides(1, 0);
                                    var forward = CalculateSides(0, 1);
                                    var backward = CalculateSides(0, -1);

                                    if (BossEncounter == 10)
                                    {
                                        roomType = Room.RoomType.Boss;
                                        BossEncounter = 0;
                                    } else if (LootingEncounter >= 4)
                                    {
                                        roomType = Room.RoomType.Looting;
                                        LootingEncounter = 0;
                                    }
                                    else
                                        roomType = Room.RoomType.Killing;
                                    BossEncounter++;
                                    LootingEncounter++;
                                    ActualRoom.PlayerInside = false;
                                    ActualRoom = new Room(roomType, d.DoorDirection, (left, forward, right, backward));
                                    ActualRoom.PlayerInside = true;
                                    MapComposition.Add((XMap, YMap), ActualRoom);
                                    RoomInitialize(ActualRoom);
                                }
                                break;
                            }
                        }
                    }

                    if (ArtefactCollecting(obj1.id, obj2.id) | ArtefactCollecting(obj2.id, obj1.id))
                    {
                        ActualRoom.EnemiesInside--;
                        break;
                    }
                    
                    isCollided = true;
                    if (obj1.initPos != Objects[obj1.id].Pos)
                        CollisionBackups(obj1.id, obj1.initPos);
                    if (obj2.initPos != Objects[obj2.id].Pos)
                        CollisionBackups(obj2.id, obj2.initPos);
                }
            }
            if (!Objects[obj1.id].IsRemoved && !Objects[obj2.id].IsRemoved)
                if (isCollided)
                {
                    Objects[obj1.id].Speed = new Vector2(0, 0);
                    Objects[obj2.id].Speed = new Vector2(0, 0);
                }
        }

        private bool CalculateSides(int d1, int d2)
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

        private bool ArtefactCollecting(int id1, int id2)
        {
            if (Objects[id1] is IArtefact a)
            { 
                if (Objects[id2] is Player p)
                {
                    if (a.Type == IArtefact.ArtefactType.Characteristics)
                    {        
                        if (a is IHP)
                            p.HP = (int)(p.HP * a.Multiplier);
                        if (a is IDMG)
                            p.DamageMultiply *= a.Multiplier;
                        if (a is ISpeed)
                            p.SpeedMultiply *= a.Multiplier;
                    }
                    else
                    {
                        (int Fire, int ice, int undead, int light) obj = (-1, -1, -1, -1);
                        foreach (var i in p.Spells)
                        {
                            if (i is Fire)
                                obj.Fire = p.Spells.IndexOf(i);
                            else if (i is Ice)
                                obj.ice = p.Spells.IndexOf(i);
                            else if (i is Undead)
                                obj.undead = p.Spells.IndexOf(i);
                            else if (i is Light)
                                obj.light = p.Spells.IndexOf(i);
                        }

                        switch (a.spell)
                        {
                            case Fire:
                                SpellStacking(obj.Fire, a.spell);
                                break;
                            case Ice:
                                SpellStacking(obj.ice, a.spell);
                                break;
                            case Undead:
                                SpellStacking(obj.undead, a.spell);
                                break;
                            case Light:
                                SpellStacking(obj.light, a.spell);
                                break;
                        }
                    }
                    Objects[id1].IsRemoved = true;
                    return true;
                }
            }
            return false;
        }

        private void SpellStacking(int id, ISpell spell)
        {
            var p = Objects[PlayerId] as Player;
            if (id != -1)
                p.Spells[id].DamageDeals = (int)(p.Spells[id].DamageDeals * 1.15f);
            else
                p.Spells.Add(spell);
        }

        private void CollisionBackups(int id, Vector2 pos)
        {
            Vector2 oppositeDir = new Vector2(0, 0);
            oppositeDir = Objects[id].Pos - pos;
            oppositeDir.Normalize();
            Objects[id].Move(Objects[id].Pos - oppositeDir);
        }
    }
}