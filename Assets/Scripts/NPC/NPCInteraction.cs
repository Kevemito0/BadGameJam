using UnityEngine;


public class NPCInteraction : MonoBehaviour, IInteractable, ILoopBreaker
{
    [Header("Dialogue")]
    [TextArea]
    public string[] dialogueLines;

    [SerializeField] private string interactionTxt;

    [Header("Time Loop")]
    [Tooltip("NPC timeLoop kırsın mı")]
    [SerializeField] private bool breaksLoop = false;

    [Tooltip("Loop sadece belirli bir koşulda kırılsın istersen buraya bağla.")]
    [SerializeField] private bool loopBreakConditionMet = true;

    public string InteractionText => interactionTxt;

    public void Interact()
    {
        if (breaksLoop && CanBreakLoop)
        {
            TimeLoopManager.Instance?.BreakLoop();
        }

        DialogueManager.Instance.StartDialogue(dialogueLines);
    }

    public bool CanBreakLoop => breaksLoop && loopBreakConditionMet;
}