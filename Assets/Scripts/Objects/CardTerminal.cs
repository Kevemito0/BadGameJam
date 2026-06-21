// Assets/Scripts/Objects/CardTerminal.cs
using UnityEngine;

public class CardTerminal : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerQuestManager questManager;

    [SerializeField] private string interactionTxt  = "Load the card [E]";
    [SerializeField] private string noCardTxt       = "Find the card";   
    [SerializeField] private string lockedTxt       = "sirayi sik";
    [SerializeField] private string doneTxt         = "card is alrdy loaded";

    public string InteractionText
    {
        get
        {
            if (questManager.cardLoaded)        return doneTxt;
            if (!questManager.hasIstanbulCard)  return noCardTxt;   
            if (!questManager.queueCleared)     return lockedTxt;
            return interactionTxt;
        }
    }

    public void Interact()
    {
        questManager.LoadCard();
    }
}