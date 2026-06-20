using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TextMeshProUGUI interactionText;
    
    private void Update()
    {
        CheckInteractable();

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.GetComponentInParent<IInteractable>() is { } interactable)
            {
                interactable.Interact();
            }
        }
    }
    
    private void CheckInteractable()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.GetComponentInParent<IInteractable>() is { } interactable)
            {
                interactionText.text = interactable.InteractionText;
                return;
            }
        }

        interactionText.text = "";
    }
}