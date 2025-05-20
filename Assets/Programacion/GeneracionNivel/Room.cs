using UnityEngine;

public class Room : MonoBehaviour
{
    private int X1pos, X2pos, Y1pos, Y2pos;
    [SerializeField] private int minX, maxX, minY, maxY;
    public int width, length;
    public float cellSize = 1f;
    public float cellQuantity = 10f;

    [SerializeField]
    GameObject floorTilePrefab, rightWallTilePrefab, leftWallTilePrefab, topWallTilePrefab, bottomWallTilePrefab;

    public Room(){
        return;
    }

    public void CreateRoom(Vector2Int origin, int minX, int maxX, int minY, int maxY)
    {

        int randomX = Random.Range(minX / 2, maxX / 2) * 2;
        int randomY = Random.Range(minY / 2, maxY / 2) * 2;

        X1pos = origin.x - randomX / 2;
        X2pos = origin.x + randomX / 2;
        Y1pos = origin.y - randomY / 2;
        Y2pos = origin.y + randomY / 2;

        width = randomX;
        length = randomY;

        Debug.Log($"Room created at {origin} with width {width} and length {length}");
    }

    public void InstatiateRoom(Vector2Int position)
    {
        for (int i = Y1pos; i < Y2pos; i++)
            for (int j = X1pos; j < X2pos; j++)
            {
                Vector3 pos = new Vector3(i * cellSize - X1pos, 0, j * cellSize - Y1pos);
                Transform w = GameObject.Instantiate(floorTilePrefab, pos, Quaternion.identity).transform;
            }
    }
}
