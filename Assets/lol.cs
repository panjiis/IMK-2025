using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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

    [Header("VR")]
    public Transform vrCamera; // Kamera dari XR rig

    private Rigidbody rb;
    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();

        if (muzzleFlash != null)
            muzzleFlash.Stop();

        if (vrCamera == null && Camera.main != null)
            vrCamera = Camera.main.transform;
    }

    void Update()
    {
        // ======== Mouse Rotation (untuk non-VR mode) ========
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        if (Camera.main != null)
            Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // ======== Mouse Shoot ========
        if (Input.GetMouseButtonDown(0))
            Shoot();

        // ======== VR Trigger Shoot ========
        if (IsRightTriggerPressed())
            Shoot();

        // ======== Rotasi tubuh mengikuti arah kamera VR pada sumbu Y saja ========
        if (vrCamera != null)
        {
            Vector3 cameraForward = vrCamera.forward;
            cameraForward.y = 0f;
            if (cameraForward != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = targetRotation;
            }
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = Vector3.zero;

        if (vrCamera != null)
        {
            Vector3 forward = vrCamera.forward;
            Vector3 right = vrCamera.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            moveDirection = right * moveX + forward * moveZ;
        }
        else
        {
            moveDirection = transform.right * moveX + transform.forward * moveZ;
        }

        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    void Shoot()
    {
        if (audioSource && shootSound)
            audioSource.PlayOneShot(shootSound);

        if (muzzleFlash)
        {
            muzzleFlash.Play();
            Invoke("StopMuzzleFlash", 0.5f);
        }

        GameObject bullet = Instantiate(bulletPrefab, gunTip.position, Quaternion.identity);

        if (vrCamera != null)
        {
            // Arah peluru mengikuti arah pandang kamera VR (headset)
            bullet.transform.rotation = Quaternion.LookRotation(vrCamera.forward);
        }
        else
        {
            bullet.transform.rotation = gunTip.rotation;
        }

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.linearVelocity = bullet.transform.forward * bulletSpeed;

        Destroy(bullet, 3f);
    }

    void StopMuzzleFlash()
    {
        if (muzzleFlash)
            muzzleFlash.Stop();
    }

    bool IsRightTriggerPressed()
    {
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (rightHand.isValid && rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed))
            return pressed;

        return false;
    }


}