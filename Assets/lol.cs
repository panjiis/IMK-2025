using UnityEngine;
using UnityEngine.XR;

public class lol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public AudioSource audioSource;
    public AudioClip shootSound;
    public ParticleSystem muzzleFlash;
    public GameObject bulletPrefab;
    public Transform gunTip;
    public float bulletSpeed = 20f;

    private Rigidbody rb;
    private bool triggerPressed = false;

    // Untuk snap turn cooldown
    private float lastSnapTime = 0f;
    public float snapCooldown = 0.4f; // detik
    public float snapAngle = 30f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (muzzleFlash != null)
            muzzleFlash.Stop();
    }

    void Update()
    {
        // Dapatkan device remote kanan
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // ===== Rotasi snap dengan joystick kanan (primary2DAxis) =====
        if (rightHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystick))
        {
            if (Time.time - lastSnapTime > snapCooldown)
            {
                if (joystick.x > 0.7f)
                {
                    transform.Rotate(0, snapAngle, 0);
                    lastSnapTime = Time.time;
                }
                else if (joystick.x < -0.7f)
                {
                    transform.Rotate(0, -snapAngle, 0);
                    lastSnapTime = Time.time;
                }
            }
        }

        // ===== Trigger kanan untuk menembak =====
        if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
        {
            if (isTriggerPressed && !triggerPressed)
            {
                Shoot();
                triggerPressed = true;
            }
            else if (!isTriggerPressed)
            {
                triggerPressed = false;
            }
        }
    }

    void FixedUpdate()
    {
        // Gerakan karakter menggunakan input keyboard (default)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection.y = 0;

        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);

        if (bulletPrefab != null && gunTip != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.linearVelocity = gunTip.forward * bulletSpeed;
            Destroy(bullet, 3f);
        }
    }
}
