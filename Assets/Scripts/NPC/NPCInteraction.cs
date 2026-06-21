using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("Diyalog")]
    [TextArea]
    public string[] dialogueLines;

    [SerializeField] private string interactionTxt;

    [Header("Time Loop")]
    [SerializeField] private GameEvent loopBreakEvent;

    [Header("Sound")]
    [SerializeField] private AudioClip talkClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float voicePitch = 1.3f;

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

        // if (audioSource != null && talkClip != null)
        //    audioSource.PlayOneShot(talkClip);

        SetShitIdle(true);

        DialogueManager.Instance.StartDialogue(dialogueLines, OnDialogueEnd, voicePitch);
    }

    private void OnDialogueEnd()
    {
        SetShitIdle(false);
    }

    private void SetShitIdle(bool value)
    {
        if (_animator != null)
            _animator.SetBool(ShitIdle, value);
    }
}