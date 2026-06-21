using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI textComp;
    [SerializeField] private float textSpeed = 0.05f;

    public bool IsOpen => dialoguePanel.activeSelf;

    private string[] lines;
    private int index;
    private Action _onComplete;   

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialoguePanel.activeSelf) return;
        if (lines == null || index >= lines.Length) return; 

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

 
    public void StartDialogue(string[] dialogueLines, Action onComplete = null)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;
        
        lines = dialogueLines;
        index = 0;
        _onComplete = onComplete;

        dialoguePanel.SetActive(true);
        textComp.text = "";

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        if (index >= lines.Length) yield break;
        
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
            StartCoroutine(TypeLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            _onComplete?.Invoke();   // diyalog bitti → NPC'ye haber ver
            _onComplete = null;
        }
    }
}