using Microsoft.Xna.Framework;
using System;


namespace game
{
    public class Player : IObject, ISolid

    {
        public ISpell QSpell;
        public ISpell WSpell;
        public ISpell ActiveSpell;

        public int HP { get; set; }
        
        public Vector2 _speed;
        public int ImageID { get; set; }
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
                if (_speed.Y > 20)
                    _speed.Y = 20;
                else if (_speed.Y < -20)
                    _speed.Y = -20;
                if (_speed.X > 20)
                    _speed.X = 20;
                else if (_speed.X < -20)
                    _speed.X = -20;
            }
        }
        public RectangleCollider Collider { get; set; }

        public Player(Vector2 position)
        {
            HP = 100;
            QSpell = new Fire();
            WSpell = new Ice();
            ActiveSpell = QSpell;
            dir = IGameplayModel.Direction.right;
            Pos = position;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 128, 128);
            IsRemoved = false;
        }

        public void Move(Vector2 pos)
        {
            Pos = pos;
            MoveCollider(Pos);
        }

        public void Update()
        {
            Pos += Speed;
            MoveCollider(Pos);
            Speed = Vector2.Multiply(Speed, 0.82f);
            if (Speed.X >= 0)
                dir = IGameplayModel.Direction.right;
            else
                dir = IGameplayModel.Direction.left;
        }

        public bool IsRemoved { get; set; }

        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 128, 128);
        }
    }

    public class Door : IObject, ISolid
    {
        public int HP { get; set; }
        public int ImageID { get; set; }
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; }
        public Vector2 Speed { get; set; }

        public Door(Vector2 postion)
        {
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

        public RectangleCollider Collider { get; set; }
        public void MoveCollider(Vector2 newPos)
        {
        }
    }

    class Wall : IObject, ISolid
    {
        public int HP { get; set; }
        public int ImageID { get; set;}
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set; }

        public Wall(Vector2 position)
        {
            IsRemoved = false;
            dir = IGameplayModel.Direction.right;
            Pos = position;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 128, 128);
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
    }

    class Goblin : IObject, ISolid, IEnemy
    {
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
            }
        }
        private int hp;
        public int ImageID { get; set; }
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; private set; }
        public Vector2 Speed { get; set; }

        public Goblin(Vector2 position)
        {
            HP = 500;
            Pos = position;
            IsRemoved = false;
            dir = IGameplayModel.Direction.right;
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
            if (currDistance > 250)
            {
                var t = MathHelper.Min((float)Math.Abs(currDistance - 70), 4f);
                Speed = Direction * t;
            }
            else
            {
                Speed = new Vector2(0, 0);
            }
        }
        
        public void Update()
        {
            Follow();
            Pos += Speed;
            MoveCollider(Pos);
            if (Speed.X >= 0)
                dir = IGameplayModel.Direction.right;
            else
                dir = IGameplayModel.Direction.left;
            if (HP <= 0)
                IsRemoved = true;
        }

        public bool IsRemoved { get; set; }
        public RectangleCollider Collider { get; set; }
        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)newPos.X, (int)newPos.Y, 128, 128);
        }

        public Player Target { get; set; }
        public int Damage { get; set; }
        public double XSpeed { get; set; }
        public double YSpeed { get; set; }
    }
}
