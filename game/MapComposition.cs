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
    public bool IsCleared;
    public RoomType roomType;
    public IGameplayModel.Direction dir;
    public (int x, int y) OldEnter = (4, 7);
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
        IsCleared = false;
        if (roomType == RoomType.StartRoom)
        {
            roomDoors.backward = false;
            dir = IGameplayModel.Direction.forward;
        }
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
}