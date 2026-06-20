using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [TextArea]
    public string[] dialogueLines;

    [SerializeField] private string interactionTxt;
    
    public string InteractionText => interactionTxt;

    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogueLines);
    }
    
    
}