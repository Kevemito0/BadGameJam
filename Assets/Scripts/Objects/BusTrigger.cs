// Assets/Scripts/Objects/BusTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class BusTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerQuestManager questManager;
    [SerializeField] private string nextSceneName = "BusScene";

    [SerializeField] private string lockedTxt    = "Otobüs kilitli";
    [SerializeField] private string noCardTxt    = "Karta ihtiyacım var [E]";  // YENİ
    [SerializeField] private string readyTxt     = "Otobüse bin [E]";

    [Header("İstanbulKart Monologu")]
    [SerializeField] private string[] noCardDialogue = new[]
    {
        "Dur bir dakika... Otobüse binmek için İstanbulKart lazım. (buraya biz yazcaz)",
        "Nerede bıraktım onu acaba? Önce kartı bulmalıyım."
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
            Debug.Log("[Bus] Henüz hazır değil.");
            return;
        }

        TimeLoopManager.Instance.BreakLoop();
    }
}