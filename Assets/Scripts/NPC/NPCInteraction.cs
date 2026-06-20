using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("Diyalog")]
    [TextArea]
    public string[] dialogueLines;

    [SerializeField] private string interactionTxt;

    [Header("Time Loop")]
    [SerializeField] private GameEvent loopBreakEvent;

    private Animator _animator;
    private static readonly int ShitIdle = Animator.StringToHash("ShitIdle");

    public string InteractionText => interactionTxt;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        if (DialogueManager.Instance.IsOpen) return;

        loopBreakEvent?.Raise();

        // Diyalog başlarken ShitIdle anim'i aç
        SetShitIdle(true);

        DialogueManager.Instance.StartDialogue(dialogueLines, OnDialogueEnd);
    }

    private void OnDialogueEnd()
    {
        // Diyalog bitince normal idle'a dön
        SetShitIdle(false);
    }

    private void SetShitIdle(bool value)
    {
        if (_animator != null)
            _animator.SetBool(ShitIdle, value);
    }
}