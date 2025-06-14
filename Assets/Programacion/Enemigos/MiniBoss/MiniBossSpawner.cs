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

        miniBoss.transform.localScale = Vector3.one * 2;
        this.gameObject.SetActive(false);

        // Set health
        EnemyHealth health = miniBoss.GetComponent<EnemyHealth>();
        health.SetHealth(health.maxHealth * 2);

        // Flag this enemy as the miniboss
        health.isMiniBoss = true;
    }
}

