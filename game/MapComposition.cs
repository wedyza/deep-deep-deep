namespace game;

public class Level
{
    
}

public class Room
{
    public char[,] Map;
    public bool IsCleared;
    public Door[] Doors;
    public RoomType roomType;
    public bool PlayerInside;
    public enum RoomType
    {
        Looting,
        Killing,
        Boss
    }

    public Room()
    {
        PlayerInside = false;
        IsCleared = false;
        
    }
}