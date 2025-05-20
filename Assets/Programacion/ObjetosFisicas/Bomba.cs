using UnityEngine;
using System.Collections;

public class Bomba : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float directHitDamage = 5f;
    [SerializeField] private float explosionDamage = 30f;

    [Header("Proximity Explosion")]
    [SerializeField] private float proximityRadius = 3f;
    [SerializeField] private float explosionDelay = 3f;

    [SerializeField] private GameObject explosionRadiusIndicatorPrefab;
    [SerializeField] private GameObject dangerRadiusIndicatorPrefab;

    private Rigidbody rb;
    private bool hasBounced = false;
    private bool hasExploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Llamado desde BombarderoController para iniciar el conteo regresivo a la explosión.
    /// </summary>
    public void IniciarCuentaRegresiva()
    {
        StartCoroutine(ExplodeAfterDelay(2f));
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasExploded) return;

        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        if (playerHealth)
        {
            playerHealth.takeDamage(directHitDamage);
        }
        else if (enemyHealth)
        {
            enemyHealth.TakeDamage(Mathf.CeilToInt(directHitDamage));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBounced)
        {
            hasBounced = true;
            // Rebote controlado al primer impacto
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 3f, rb.linearVelocity.z);
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        DealExplosionDamage(transform.position);
        ShowExplosionRadius();
    }

    private void DealExplosionDamage(Vector3 bombPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(bombPosition, proximityRadius);

        foreach (Collider collider in colliders)
        {
            if (Physics.Linecast(bombPosition, collider.transform.position, out RaycastHit hit))
            {
                if (hit.collider == collider)
                {
                    collider.GetComponentInParent<PlayerHealth>()?.takeDamage(explosionDamage);
                    collider.GetComponentInParent<EnemyHealth>()?.TakeDamage(Mathf.CeilToInt(explosionDamage));
                }
            }
        }
    }

    private void ShowExplosionRadius()
    {
        if (explosionRadiusIndicatorPrefab)
        {
            GameObject indicator = Instantiate(
                explosionRadiusIndicatorPrefab,
                transform.position,
                Quaternion.identity
            );

            float scale = proximityRadius * 2f;
            indicator.transform.localScale = new Vector3(scale, scale, scale);

            Destroy(indicator, 0.5f);
        }

        Destroy(gameObject); // Destruir la bomba después de mostrar explosión
    }
}
