// Assets/Scripts/Objects/CardTerminal.cs
using UnityEngine;

public class CardTerminal : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerQuestManager questManager;

    [SerializeField] private string interactionTxt  = "Kartı yükle [E]";
    [SerializeField] private string noCardTxt       = "Önce İstanbulKart'ı bul";   // YENİ
    [SerializeField] private string lockedTxt       = "Önce sırayı geç";
    [SerializeField] private string doneTxt         = "Kart zaten yüklü";

    public string InteractionText
    {
        get
        {
            if (questManager.cardLoaded)        return doneTxt;
            if (!questManager.hasIstanbulCard)  return noCardTxt;   // YENİ kontrol
            if (!questManager.queueCleared)     return lockedTxt;
            return interactionTxt;
        }
    }

    public void Interact()
    {
        questManager.LoadCard();
    }
}