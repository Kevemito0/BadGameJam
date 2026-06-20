using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI textComp;
    [SerializeField] private float textSpeed = 0.05f;

    private string[] lines;
    private int index;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialoguePanel.activeSelf)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (textComp.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComp.text = lines[index];
            }
        }
    }

    public void StartDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        index = 0;

        dialoguePanel.SetActive(true);
        textComp.text = "";

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
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
        }
    }
}