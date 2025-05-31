using UnityEngine;

public class PetFollow : MonoBehaviour
{
    public float followDistance = 5f;
    public float heightOffset = 1f; 
    public float moveSpeed = 4f;
    public float rotationSpeed = 2f;

    private Transform player;

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        Vector3 offset = -player.forward * followDistance + Vector3.up * heightOffset;
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

      
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

       
            Quaternion modelCorrection = Quaternion.Euler(-90f, 90f, 0f);

            Quaternion finalRotation = lookRotation * modelCorrection;

            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, rotationSpeed * Time.deltaTime);
        }

    }
}
