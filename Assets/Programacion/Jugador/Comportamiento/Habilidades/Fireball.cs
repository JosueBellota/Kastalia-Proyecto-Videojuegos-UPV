using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float damage;
    float areaOfEffect;

    public void SetDamage(float dmg) => damage = dmg;
    public void SetAreaOfEffect(float area) { areaOfEffect = area;}

    private void OnTriggerEnter(Collider other)
    {
        DealExplosionDamage(transform.position);
        Destroy(gameObject);
    }

    private void DealExplosionDamage(Vector3 bombPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(bombPosition, areaOfEffect);

        foreach (Collider collider in colliders)
        {
            if (Physics.Linecast(bombPosition, collider.transform.position, out RaycastHit hit))
            {
                if (hit.collider == collider)
                {
                    collider.GetComponentInParent<EnemyHealth>()?.TakeDamage(Mathf.CeilToInt(damage));
                }
            }
        }
    }
}

