using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float stopDistance = 2f;

    private bool shouldFollow = false;

    public bool hasBeenShot = false;  // Penanda sudah tertembak

    public void ActivateFollow(Transform targetPlayer)
    {
        player = targetPlayer;
        shouldFollow = true;
    }

    public void DeactivateFollow()
    {
        shouldFollow = false;
    }

    void Update()
    {
        if (shouldFollow && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > stopDistance)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }
}
