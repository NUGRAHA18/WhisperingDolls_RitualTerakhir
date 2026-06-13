using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRange = 2f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] Transform cameraTransform;

    public IInteractable CurrentTarget { get; private set; }
    public string CurrentPrompt => CurrentTarget?.InteractPrompt ?? string.Empty;

    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;

        DetectInteractable();

        if (Input.GetKeyDown(KeyCode.E) && CurrentTarget != null)
            CurrentTarget.Interact(this);
    }

    void DetectInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
            CurrentTarget = hit.collider.GetComponent<IInteractable>();
        else
            CurrentTarget = null;
    }

    public Transform CameraTransform => cameraTransform;
}
