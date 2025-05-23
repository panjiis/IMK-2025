using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyFPSController : MonoBehaviour
{
    public float speed = 100f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    private Rigidbody rb;
    private float verticalLook = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent Rigidbody from rotating due to physics
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player horizontally (yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically (pitch)
        verticalLook -= mouseY;
        verticalLook = Mathf.Clamp(verticalLook, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(verticalLook, 0f, 0f);
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Use camera's forward/right direction for movement (ignore vertical)
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (right * moveX + forward * moveZ).normalized;

        Vector3 velocity = move * speed;
        velocity.y = rb.linearVelocity.y; // Keep Y (gravity)

        rb.linearVelocity = velocity;
    }
}
