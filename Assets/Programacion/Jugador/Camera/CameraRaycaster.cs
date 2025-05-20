using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Transform player;

    void Start()
    {
        findPlayerTransform();
    }

    void Update()
    {
        findPlayerTransform();
        if(!player) return;

        RaycastHit[] hits = Physics.RaycastAll(
            transform.position,
            player.position - transform.position,
            Vector3.Distance(transform.position, player.position),
            layerMask
        );

        if (hits.Length > 0)
        {
            foreach (var item in hits)
            {
                if (item.transform.TryGetComponent<ObstaculoTransparente>(out ObstaculoTransparente obstaculo))
                {
                    obstaculo.hitted = true;
                }
            }
        }
    }

    private void findPlayerTransform()
    {
        GameObject foundPlayer = GameObject.FindWithTag("Player");
        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }
    }
}
