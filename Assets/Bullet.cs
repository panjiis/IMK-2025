using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target")) // Jika objek memiliki Tag "Target"
        {
            Destroy(other.gameObject); // Hancurkan objek target
            Destroy(gameObject); // Hancurkan peluru
        }
    }
}
