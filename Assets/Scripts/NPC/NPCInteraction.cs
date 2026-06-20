using UnityEngine;


public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("Diyalog")]
    [TextArea]
    public string[] dialogueLines;

    [SerializeField] private string interactionTxt;

    [Header("Time Loop")]
    [SerializeField] private GameEvent loopBreakEvent;

    public string InteractionText => interactionTxt;

    public void Interact()
    {
        // Event varsa raise et (TimeLoopManager dinliyor)
        loopBreakEvent?.Raise();

        DialogueManager.Instance.StartDialogue(dialogueLines);
    }
}