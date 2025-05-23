using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SheepRayFollower : MonoBehaviour
{
    public XRController rightHandController;
    public Transform player; // Player Transform to follow

    public float rayLength = 10f;
    public LayerMask rayLayerMask;
    public float followSpeed = 5f;

    private GameObject followingSheep = null;

    void Update()
    {
        // Check if trigger is pressed
        if (rightHandController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool isPressed) && isPressed)
        {
            // Cast a ray
            if (Physics.Raycast(rightHandController.transform.position, rightHandController.transform.forward, out RaycastHit hit, rayLength, rayLayerMask))
            {
                if (hit.collider.CompareTag("Sheep"))
                {
                    followingSheep = hit.collider.gameObject;
                }
            }
        }

        // Make sheep follow player
        if (followingSheep != null)
        {
            Vector3 targetPos = player.position;
            targetPos.y = followingSheep.transform.position.y; // Keep sheep on ground
            followingSheep.transform.position = Vector3.MoveTowards(followingSheep.transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }
}
