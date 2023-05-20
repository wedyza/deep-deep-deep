using Microsoft.Xna.Framework;
using System;


namespace game
{
    class Player : IObject, ISolid

    {
        public ISpell QSpell;
        public ISpell WSpell;
        public ISpell ActiveSpell;

        private int HP { get; set; }
        
        public Vector2 _speed;
        public int ImageID { get; set; }
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; private set; }

        public bool Enemy { get; }
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
            Enemy = false;
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

    class Door : IObject, ISolid
    {
        public int ImageID { get; set; }
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; }
        public bool Enemy { get; }
        public Vector2 Speed { get; set; }

        public Door(Vector2 postion)
        {
            Pos = postion;
            dir = IGameplayModel.Direction.right;
            Enemy = false;
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
        public int ImageID { get; set;}
        public IGameplayModel.Direction dir { get; set; }
        public Vector2 Pos { get; set; }
        public bool Enemy { get;  }
        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set; }

        public Wall(Vector2 position)
        {
            IsRemoved = false;
            dir = IGameplayModel.Direction.right;
            Pos = position;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 128, 128);
            Enemy = false;
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
}
