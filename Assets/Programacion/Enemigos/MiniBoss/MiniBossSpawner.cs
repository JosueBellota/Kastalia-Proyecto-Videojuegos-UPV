using UnityEngine;
using UnityEngine.AI;

public class MiniBossSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    GameObject miniBoss;

    void Start()
    {
        int randomIndex = Random.Range(0, enemies.Length);
        miniBoss = Instantiate(enemies[randomIndex], transform.position, Quaternion.identity);
        miniBoss.transform.localScale = new Vector3(1, 1, 1) *2;
        this.gameObject.SetActive(false);
        miniBoss.GetComponent<NavMeshAgent>().speed /= 2;
        miniBoss.GetComponent<EnemyHealth>().SetHealth(2*miniBoss.GetComponent<EnemyHealth>().maxHealth);
    }
}
