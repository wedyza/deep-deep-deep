using System;
using Microsoft.Xna.Framework;

namespace game;

public interface IRooms
{
    
}

public class Nothing : IRooms
{
    
}

public class Room : IRooms
{
    public char[,] Map;
    public RoomType roomType;
    public IGameplayModel.Direction dir;
    public (int x, int y) OldEnter = (7, 4);
    public int EnemiesInside;
    public bool PlayerInside;
    public enum RoomType
    {
        Looting,
        Killing,
        Boss,
        StartRoom
    }

    public Room(RoomType _roomType, IGameplayModel.Direction DoorDirection, (bool left, bool forward, bool right, bool backward) roomDoors)
    {
        dir = DoorDirection;
        roomType = _roomType;
        Map = new char[15, 9];
        RoomPresets();
        createWallsOnEdges();
        CreateDoors(roomDoors);
    }

    public void PlacePlayer()
    {
        Map[OldEnter.x, OldEnter.y] = '\0';
        switch (dir)
        {
            case IGameplayModel.Direction.right:
                Map[1, 4] = 'P';
                OldEnter = (1, 4);
                break;
            case IGameplayModel.Direction.left:
                Map[13, 4] = 'P';
                OldEnter = (13, 4);
                break;
            case IGameplayModel.Direction.backward:
                Map[7, 2] = 'P';
                OldEnter = (7, 2);
                break;
            case IGameplayModel.Direction.forward:
                Map[7, 7] = 'P';
                OldEnter = (7, 7);
                break;
            case IGameplayModel.Direction.none:
                Map[7, 4] = 'P';
                OldEnter = (7, 4);
                break;
        }
    }
    
    public void CreateDoors((bool left, bool forward, bool right, bool backward) roomDoors)
    {
        if (roomDoors.left)
            Map[0, 4] = 'D';
        if (roomDoors.forward)
            Map[7, 0] = 'D';
        if (roomDoors.right)
            Map[14, 4] = 'D';
        if (roomDoors.backward)
            Map[7, 8] = 'D';
    }

    public void createWallsOnEdges()
    {
        for (int x = 0; x < Map.GetLength(0); x++)
        {
            Map[x, 0] = 'W';
            Map[x, Map.GetLength(1) - 1] = 'W';
        }

        for (int y = 1; y < Map.GetLength(1); y++)
        {
            Map[0, y] = 'W';
            Map[Map.GetLength(0) - 1, y] = 'W';
        }
    }

    public void RoomPresets()
    {
        if (roomType == RoomType.StartRoom)
        {
            PlayerInside = true;
            EnemiesInside = 0;
            Map[1, 1] = 'T';
        }
        else if (roomType == RoomType.Looting)
        {
            EnemiesInside = 1;
            Map[6, 6] = 'A';
        }
        else if (roomType == RoomType.Killing)
        {
            var s = new Random();
            int n = s.Next(3);
            switch (n)
            {
                case 0:
                    Map[5, 3] = 'G';
                    Map[8, 4] = 'G';
                    EnemiesInside = 2;
                    break;
                case 1:
                    Map[7, 5] = 'G';
                    Map[10, 6] = 'G';
                    Map[8, 4] = 'G';
                    EnemiesInside = 3;
                    break;
                case 2:
                    Map[8, 4] = 'G';
                    Map[10, 7] = 'G';
                    EnemiesInside = 2;
                    break;
            }
        }
        else if (roomType == RoomType.Boss)
        {
            Map[8, 4] = 'B';
            EnemiesInside = 2;
        }
    }
}

public class Tutorial : IObject
{
    public int HP { get; set; }
    public int ImageID { get; set; }
    public float SpeedMultiply { get; set; }
    public double DamageMultiply { get; set; }
    public IGameplayModel.Direction dir { get; set; }
    public Vector2 Pos { get; }
    public Vector2 Speed { get; set; }
    public void Move(Vector2 pos)
    {
    }

    public Tutorial(Vector2 Pos)
    {
        this.Pos = Pos;
        dir = IGameplayModel.Direction.right;
        ImageID = (byte)GameCycle.ObjectTypes.tutorial;
    }
    
    public void Update()
    {
    }

    public bool IsRemoved { get; set; }
    public ISpell.MagicType UnderEffect { get; set; }
}