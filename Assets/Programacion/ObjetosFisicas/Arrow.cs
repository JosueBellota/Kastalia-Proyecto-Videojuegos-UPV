using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Blood Effect Settings")]
    [SerializeField] private int bloodParticles = 15; // Increased from 8
    [SerializeField] private float bloodForce = 1.2f; // Increased from 0.5f
    [SerializeField] private float bloodSize = 0.15f; // Increased from 0.1f
    [SerializeField] private Color bloodColor = new Color(0.8f, 0f, 0f, 1f); // Darker red
    [SerializeField] private float bloodDuration = 2f; // Longer duration
    [SerializeField] private float bloodSplatterSize = 0.75f; // Added for main splatter
    
    [Header("Arrow Settings")]
    [SerializeField] private float TTL = 3f;
    
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
        hasHit = true;
        
        Vector3 hitPoint = other.ClosestPoint(transform.position);
        
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
        
        //  Create main blood splatter
        // CreateMainBloodSplatter(hitPoint, other.transform);
        
        StickToTarget(other.transform);
    }

    //Fédor: "Nada de esto funciona como esperado, crea charcos enormes que ocupan la pantalla entera y a veces dejan el juego en rojo"

    // private void CreateBloodEffect(Vector3 hitPoint)
    // {
    //     for (int i = 0; i < bloodParticles; i++)
    //     {
    //         StartCoroutine(CreateBloodParticle(hitPoint));
    //     }
    // }

    // private void CreateMainBloodSplatter(Vector3 position, Transform parent)
    // {
    //     GameObject mainSplatter = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //     mainSplatter.name = "MainBloodSplatter";
    //     mainSplatter.transform.position = position + new Vector3(0, 0, -0.01f);
    //     mainSplatter.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
    //     mainSplatter.transform.parent = parent;

    //     // Create a simple red texture programmatically
    //     Texture2D bloodTex = new Texture2D(1, 1);
    //     bloodTex.SetPixel(0, 0, new Color(0.9f, 0f, 0f, 0.7f));
    //     bloodTex.Apply();

    //     var renderer = mainSplatter.GetComponent<Renderer>();
    //     renderer.material = new Material(Shader.Find("Sprites/Default"));
    //     renderer.material.mainTexture = bloodTex;
    //     renderer.material.color = Color.red;

    //     Destroy(mainSplatter.GetComponent<Collider>());

    //     float size = Random.Range(bloodSplatterSize * 0.8f, bloodSplatterSize * 1.2f);
    //     mainSplatter.transform.localScale = new Vector3(size, size * 0.7f, size);
    //     mainSplatter.transform.Rotate(0, 0, Random.Range(0, 360f));

    //     Destroy(mainSplatter, bloodDuration * 1.5f);
    // }

    // private IEnumerator CreateBloodParticle(Vector3 position)
    // {
    //     GameObject blood = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //     blood.name = "BloodParticle";
    //     blood.transform.position = position;
    //     blood.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

    //     // Create red texture for particle
    //     Texture2D particleTex = new Texture2D(1, 1);
    //     particleTex.SetPixel(0, 0, Color.red);
    //     particleTex.Apply();

    //     var renderer = blood.GetComponent<Renderer>();
    //     renderer.material = new Material(Shader.Find("Sprites/Default"));
    //     renderer.material.mainTexture = particleTex;
    //     renderer.material.color = new Color(0.8f, 0f, 0f, Random.Range(0.7f, 1f));

    //     Destroy(blood.GetComponent<Collider>());

    //     var rb = blood.AddComponent<Rigidbody>();
    //     rb.AddForce(new Vector3(
    //         Random.Range(-bloodForce * 2f, bloodForce * 2f),
    //         Random.Range(bloodForce * 0.5f, bloodForce * 1.5f),
    //         Random.Range(-bloodForce * 2f, bloodForce * 2f)
    //     ), ForceMode.Impulse);

    //     rb.AddTorque(new Vector3(
    //         Random.Range(-10f, 10f),
    //         Random.Range(-10f, 10f),
    //         Random.Range(-10f, 10f)
    //     ));

    //     float size = Random.Range(bloodSize * 0.7f, bloodSize * 1.5f);
    //     blood.transform.localScale = new Vector3(size, size, size);

    //     float elapsed = 0f;
    //     Color startColor = renderer.material.color;
    //     Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

    //     while (elapsed < bloodDuration)
    //     {
    //         float t = elapsed / bloodDuration;
    //         renderer.material.color = Color.Lerp(startColor, endColor, t);
    //         blood.transform.localScale = Vector3.Lerp(
    //             blood.transform.localScale, 
    //             Vector3.zero, 
    //             t * 0.5f
    //         );
    //         elapsed += Time.deltaTime;
    //         yield return null;
    //     }

    //     Destroy(blood);
    //     Destroy(particleTex);
    // }
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