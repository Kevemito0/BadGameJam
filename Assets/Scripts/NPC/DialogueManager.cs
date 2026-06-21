using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI textComp;
    [SerializeField] private float textSpeed = 0.05f;

    [Header("Dialogue Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] talkSounds;
    [SerializeField] private float pitch = 1.3f;

    private string[] lines;
    private int index;
    private Action onDialogueEnd;   // callback

    public bool IsOpen => dialoguePanel.activeSelf;   // IsOpen property

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!IsOpen) return;
        if (lines == null || lines.Length == 0) return;        
        if (index < 0 || index >= lines.Length) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (textComp.text == lines[index])
                NextLine();
            else
            {
                StopAllCoroutines();
                textComp.text = lines[index];
            }
        }
    }

    // Callback olmadan da çağrılabilsin diye overload
    public void StartDialogue(string[] dialogueLines)
        => StartDialogue(dialogueLines, null);

    public void StartDialogue(string[] dialogueLines, Action onEnd = null, float pitch = 1f)
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogWarning("DialogueManager: Diyalog satırları boş!");
            return;
        }

        lines = dialogueLines;
        index = 0;
        onDialogueEnd = onEnd;

        if (audioSource != null)
            audioSource.pitch = pitch;      // gelen pitch'i uygula

        dialoguePanel.SetActive(true);
        textComp.text = "";

        PlayRandomTalkSound();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        if (index < 0 || index >= lines.Length) yield break;
        
        foreach (char c in lines[index])
        {
            textComp.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComp.text = "";
            PlayRandomTalkSound();
            StartCoroutine(TypeLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            onDialogueEnd?.Invoke();   // callback'i tetikle
            onDialogueEnd = null;
        }
    }

    void PlayRandomTalkSound()
    {
        if (audioSource == null || talkSounds == null || talkSounds.Length == 0) return;

        audioSource.PlayOneShot(talkSounds[UnityEngine.Random.Range(0, talkSounds.Length)]);
    }
}