using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    

    void Awake()
    {
        Room room = new Room();
        room.CreateRoom(new Vector2Int(0, 0), 10, 20, 10, 20);
        room.InstatiateRoom(new Vector2Int(0, 0));
    }

    void Update()
    {
        
    }
}
