using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Timers;

namespace game
{
    public class Player : IObject, ISolid

    {
        public ISpell ActiveSpell;
        public List<ISpell> Spells;
        public int HP { get; set; }
        
        public Vector2 _speed;
        public int ImageID { get; set; }
        public float SpeedMultiply { get; set; }
        public double DamageMultiply { get; set; }
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; private set; }
        public Vector2 Speed
        {
            get
            {
                return _speed;
            }

            set
            {
                _speed = value;
                if (_speed.Y > SpeedMultiply)
                    _speed.Y = SpeedMultiply;
                else if (_speed.Y < -SpeedMultiply)
                    _speed.Y = -SpeedMultiply;
                if (_speed.X > SpeedMultiply)
                    _speed.X = SpeedMultiply;
                else if (_speed.X < -SpeedMultiply)
                    _speed.X = -SpeedMultiply;
            }
        }
        public RectangleCollider Collider { get; set; }

        public Player()
        {
            Spells = new List<ISpell>{new Fire()};
            DamageMultiply = 1;
            SpeedMultiply = 20;
            UnderEffect = ISpell.MagicType.none;
            HP = 100;
            ActiveSpell = new Fire();
            dir = IGameplayModel.Direction.right;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 128, 128);
            ImageID = (byte)GameCycle.ObjectTypes.player;
            IsRemoved = false;
        }

        public void Move(Vector2 pos)
        {
            Pos = pos;
            MoveCollider(Pos);
        }

        public void Update()
        {
            if (HP <= 0)
                IsRemoved = true;
            Pos += Speed;
            MoveCollider(Pos);
            Speed = Vector2.Multiply(Speed, 0.82f);
            if (Speed.X >= 0)
                dir = IGameplayModel.Direction.right;
            else
                dir = IGameplayModel.Direction.left;
        }

        public bool IsRemoved { get; set; }

        public ISpell.MagicType UnderEffect { get; set; }

        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 128, 128);
        }
    }

    public class Door : IObject, ISolid
    {
        public int HP { get; set; }
        public int ImageID { get; set; }
        public float SpeedMultiply { get; set; }
        public double DamageMultiply { get; set; }
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; }
        public Vector2 Speed { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public IGameplayModel.Direction DoorDirection { get; set; }
        public Door(Vector2 postion)
        {
            UnderEffect = ISpell.MagicType.none;
            Pos = postion;
            dir = IGameplayModel.Direction.right;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 128, 128);
            IsRemoved = false;
        }
        public void Move(Vector2 pos)
        {
        }

        public void Update()
        {
        }

        public bool IsRemoved { get; set; }

        public ISpell.MagicType UnderEffect { get; set; }
        public RectangleCollider Collider { get; set; }
        public void MoveCollider(Vector2 newPos)
        {
        }
    }

    class Wall : IObject, ISolid
    {
        public int HP { get; set; }
        public int ImageID { get; set;}
        public float SpeedMultiply { get; set; }
        public double DamageMultiply { get; set; }
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set; }

        public Wall(Vector2 position)
        {
            UnderEffect = ISpell.MagicType.none;
            dir = IGameplayModel.Direction.right;
            Pos = position;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 128, 128);
            IsRemoved = false;
        }
        public void MoveCollider(Vector2 newPos)
        {
        }

        public void Move(Vector2 pos)
        {
        }

        public void Update()
        {
        }

        public bool IsRemoved { get; set; }

        public ISpell.MagicType UnderEffect { get; set; }
    }

    class Goblin : IObject, ISolid, IEnemy
    {
        public int HP { get; set; }
        public int ImageID { get; set; }
        public float SpeedMultiply { get; set; }
        public double DamageMultiply { get; set; }
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; private set; }
        public Vector2 Speed { get; set; }

        void EffectCheck()
        {
            if (UnderEffect != ISpell.MagicType.none)
            {
                if (UnderEffect == ISpell.MagicType.fire)
                {
                    DamageMultiply = 1.07f;
                    SpeedMultiply = 1f;
                }
                else if (UnderEffect == ISpell.MagicType.ice)
                {
                    DamageMultiply = 1f;
                    SpeedMultiply = .7f;
                }
                else if (UnderEffect == ISpell.MagicType.death)
                {
                    SpeedMultiply = 1f;
                    DamageMultiply = 1.4f;
                }
                else if (UnderEffect == ISpell.MagicType.light)
                {
                    SpeedMultiply = .9f;
                    DamageMultiply = 1.15f;
                }
            }
        }
        
        public Goblin(Vector2 position)
        {
            SpeedMultiply = 1f;
            DamageMultiply = 1;
            UnderEffect = ISpell.MagicType.none;
            HP = 500;
            Pos = position;
            dir = IGameplayModel.Direction.right;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 128, 128);
            IsRemoved = false;
        }
        
        public void Move(Vector2 pos)
        {
            Pos = pos;
        }

        public void Follow()
        {
            var distance = Target.Pos - Pos;
            var rotation = (float)Math.Atan2(distance.Y, distance.X);

            var Direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            var currDistance = Vector2.Distance(Pos, Target.Pos);
            if (currDistance > 195)
            {
                var t = MathHelper.Min((float)Math.Abs(currDistance - 250), 6f);
                Speed = Direction * t * SpeedMultiply;
            }
            else
            {
                DAMAGEDEAL();
                Speed = new Vector2(0, 0);
            }
        }

        public void DAMAGEDEAL()
        {
            var aTimer = new Timer();
            aTimer.Interval = 1000;
            aTimer.Elapsed += (sender, args) => Target.HP -= 1;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }
        
        public void Update()
        {
            Follow();
            Move(Pos + Speed);
            MoveCollider(Pos);
            EffectCheck();
            if (Speed.X > 0)
                dir = IGameplayModel.Direction.right;
            else if (Speed.X < 0)
                dir = IGameplayModel.Direction.left;
        }

        public bool IsRemoved { get; set; }


        public ISpell.MagicType UnderEffect { get; set; }
        public RectangleCollider Collider { get; set; }
        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)newPos.X, (int)newPos.Y, 128, 128);
        }

        public Player Target { get; set; }
        public int Damage { get; set; }
    }

    class Boss : IEnemy, IObject, ISolid
    {
        public Player Target { get; set; }
        public int Damage { get; set; }
        public IArtefact Artefact { get; set; }
        public int HP { get; set; }
        public int ImageID { get; set; }
        public float SpeedMultiply { get; set; }
        public double DamageMultiply { get; set; }
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; private set; }
        public Vector2 Speed { get; set; }

        public void Follow()
        {
            var distance = Target.Pos - Pos;
            var rotation = (float)Math.Atan2(distance.Y, distance.X);

            var Direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            var currDistance = Vector2.Distance(Pos, Target.Pos);
            if (currDistance > 250)
            {
                var t = MathHelper.Min((float)Math.Abs(currDistance - 250), 6f);
                Speed = Direction * t * SpeedMultiply;
            }
            else
            {
                Speed = new Vector2(0, 0);
            }
        }
        
        public void Move(Vector2 pos)
        {
            Pos = pos;
        }

        public void Update()
        {
            Follow();
            Move(Pos + Speed);
            MoveCollider(Pos);
            EffectCheck();
            GenerateRandomArtefact();
            if (Speed.X > 0)
                dir = IGameplayModel.Direction.right;
            else if (Speed.X < 0)
                dir = IGameplayModel.Direction.left;
        }

        public bool IsRemoved { get; set; }

        void GenerateRandomArtefact()
        {
            var rand = new Random();
            int num = rand.Next(4);
            switch (num)
            {
                case 0:
                    Artefact = new BookOfIcycle(Pos);
                    break;
                case 1:
                    Artefact = new BookOfLighters(Pos);
                    break;
                case 2:
                    Artefact = new BookOfNecromancer(Pos);
                    break;
                case 3:
                    Artefact = new BookOfPyromancer(Pos);
                    break;
            }
        }
        
        void EffectCheck()
        {
            if (UnderEffect != ISpell.MagicType.none)
            {
                if (UnderEffect == ISpell.MagicType.fire)
                {
                    DamageMultiply = 1.07f;
                    SpeedMultiply = 1f;
                }
                else if (UnderEffect == ISpell.MagicType.ice)
                {
                    DamageMultiply = 1f;
                    SpeedMultiply = .7f;
                }
                else if (UnderEffect == ISpell.MagicType.death)
                {
                    SpeedMultiply = 1f;
                    DamageMultiply = 1.4f;
                }
                else if (UnderEffect == ISpell.MagicType.light)
                {
                    SpeedMultiply = .9f;
                    DamageMultiply = 1.15f;
                }
            }
        }

        public Boss(Vector2 pos)
        {
            Pos = pos;
            ImageID = (byte)GameCycle.ObjectTypes.boss;
            UnderEffect = ISpell.MagicType.none;
            SpeedMultiply = 1f;
            DamageMultiply = 1f;
            HP = 2000;
            dir = IGameplayModel.Direction.right;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 192, 192);
            IsRemoved = false;
        }
        
        public ISpell.MagicType UnderEffect { get; set; }
        public RectangleCollider Collider { get; set; }
        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)newPos.X, (int)newPos.Y, 192, 192);
        }
    }
}