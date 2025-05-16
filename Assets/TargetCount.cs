using UnityEngine;
using TMPro;

public class TargetCount : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2f, 0);
    public TextMeshProUGUI countText;

    void Update()
    {
        // UI mengikuti posisi world player dan menghadap kamera
        // Cegah error jika player belum di-assign
        if (player != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position + offset);
            transform.position = screenPos;
        }

        int count1 = GameObject.FindGameObjectsWithTag("Target").Length;
        int count2 = GameObject.FindGameObjectsWithTag("Target2").Length;
        int count3 = GameObject.FindGameObjectsWithTag("Target3").Length;

        if (countText != null)
        {
            countText.text = $"Target1: {count1}\nTarget2: {count2-1}\nTarget3: {count3}";
        }
    }
}
