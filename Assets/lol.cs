using UnityEngine;

public class lol : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public AudioSource audioSource;
    public AudioClip shootSound;
    public ParticleSystem muzzleFlash; // Efek visual tembakan
    public GameObject bulletPrefab; // Prefab peluru
    public Transform gunTip; // Posisi awal peluru
    public float bulletSpeed = 0.0005f;

    private Rigidbody rb;
    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();

        if (muzzleFlash != null)
        {
            muzzleFlash.Stop();
        }
    }

    void Update()
    {
        // ======== ROTASI KAMERA ========
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // ======== TEMBAKAN ========
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // ======== GERAKAN KARAKTER ========
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection.y = 0;

        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    void Shoot()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
            Invoke("StopMuzzleFlash", 0.5f);
        }

        // Spawn peluru
        GameObject bullet = Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.linearVelocity = gunTip.forward * bulletSpeed; // Maju lurus sesuai arah pandangan

        Destroy(bullet, 3f); // Hapus peluru setelah 3 detik agar tidak memenuhi scene
    }

    void StopMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Stop();
        }
    }
}
