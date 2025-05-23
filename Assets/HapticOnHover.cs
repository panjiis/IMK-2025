using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class HapticOnHover : MonoBehaviour
{
    public float amplitude = 0.5f; // kekuatan getaran (0.0 - 1.0)
    public float duration = 0.1f;  // lama getaran dalam detik

    private void OnEnable()
    {
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEntered);
    }

    private void OnDisable()
    {
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        // Pastikan interactor mendukung haptics
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
        {
            var controller = controllerInteractor.xrController;
            if (controller != null)
            {
                controller.SendHapticImpulse(amplitude, duration);
            }
        }
    }
}
