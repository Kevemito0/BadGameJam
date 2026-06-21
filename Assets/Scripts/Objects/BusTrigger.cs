// Assets/Scripts/Objects/BusTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class BusTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerQuestManager questManager;
    [SerializeField] private string nextSceneName = "BusScene";

    [SerializeField] private string lockedTxt    = "Locked";
    [SerializeField] private string noCardTxt    = "You need Bus Card [E]";  
    [SerializeField] private string readyTxt     = "Get in [E]";

    [SerializeField] private string[] noCardDialogue = new[]
    {
        "asd",
        "dsa"
    };

    public string InteractionText
    {
        get
        {
            if (questManager.AllQuestsComplete)    return readyTxt;
            if (!questManager.hasIstanbulCard)     return noCardTxt;   // YENİ
            return lockedTxt;
        }
    }

    public void Interact()
    {
        
        if (!questManager.hasIstanbulCard)
        {
            if (DialogueManager.Instance != null && !DialogueManager.Instance.IsOpen)
                DialogueManager.Instance.StartDialogue(noCardDialogue);
            return;
        }

        if (!questManager.cardLoaded)
        {
            SoundManager.PlaySound(SoundType.CardDeclined);
            return;
        }
        
        if (!questManager.AllQuestsComplete)
        {
            return;
        }

        TimeLoopManager.Instance.BreakLoop();
    }
}