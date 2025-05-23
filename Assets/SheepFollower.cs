using UnityEngine;

public class SheepFollower : MonoBehaviour
{
    public Transform player;         // XR Origin
    public float followSpeed = 2f;
    public float stopDistance = 1.5f;
    

    public void FollPlayer()
    {
        if (player == null) return;

        // Jarak antara sheep dan player
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
