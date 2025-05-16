using UnityEngine;

public class laser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform firePoint;  // Titik awal laser
    public float laserDistance = 0.0001f;  // Jarak maksimal laser

    void Start()
    {
        lineRenderer.enabled = false; // Sembunyikan laser di awal
    }

    void Update()
    {
        if (Input.GetMouseButton(0))  // Klik kiri untuk menembak
        {
            ShootLaser();
        }
        else
        {
            lineRenderer.enabled = false;  // Sembunyikan laser jika tidak menembak
        }
    }

    void ShootLaser()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position); // Posisi awal laser

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, laserDistance))
        {
            lineRenderer.SetPosition(1, hit.point); // Jika kena objek, laser berhenti di sana
        }
        else
        {
            lineRenderer.SetPosition(1, firePoint.position + firePoint.forward * laserDistance); // Jika tidak kena, tembak lurus
        }
    }
}
