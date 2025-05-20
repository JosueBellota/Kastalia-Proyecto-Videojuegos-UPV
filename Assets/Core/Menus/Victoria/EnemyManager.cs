using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    private int totalEnemies = 0;
    private int enemiesRemaining = 0;

    private void Awake()
    {
        // se asegura de que solo haya una instancia
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    

    // Este método se puede seguir usando si se generan enemigos después del inicio
    public void RegisterEnemy()
    {
        totalEnemies++;
        enemiesRemaining++;
    }

    // Llama este método cuando un enemigo muere
    public void UnregisterEnemy()
    {

        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            GameManager.instance.WinGame();
        }
    }
}
