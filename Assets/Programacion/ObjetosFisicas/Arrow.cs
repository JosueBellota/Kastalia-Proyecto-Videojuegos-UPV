using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    [Header("Arrow Settings")]
    [SerializeField] private float TTL = 1f;
    
    private Rigidbody rb;
    private bool hasHit = false;
    private float damage;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyArrowAfterTime(TTL));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other is SphereCollider) return;

        hasHit = true;

        // ✅ Esta línea es segura para todos los tipos de Collider
        Vector3 hitPoint = other.bounds.ClosestPoint(transform.position);

        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        if (playerHealth)
        {
            playerHealth.takeDamage(damage);
            // CreateBloodEffect(hitPoint);
        }
        else if (enemyHealth)
        {
            enemyHealth.TakeDamage(Mathf.CeilToInt(damage));
            // CreateBloodEffect(hitPoint);
        }

        StickToTarget(other.transform);
    }
     public void setDamage(float damage)
    {
        this.damage = damage;
    }

    private void StickToTarget(Transform target)
    {
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        transform.SetParent(target);
    }

    IEnumerator DestroyArrowAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}