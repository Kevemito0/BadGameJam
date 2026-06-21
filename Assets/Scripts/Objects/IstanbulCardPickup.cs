// Assets/Scripts/Objects/IstanbulCardPickup.cs
using UnityEngine;

public class IstanbulCardPickup : MonoBehaviour, IInteractable
{
    [Header("Quest")]
    [SerializeField] private PlayerQuestManager questManager;

    [Header("Metinler")]
    [SerializeField] private string interactionTxt  = "İstanbulKart'ı al [E]";
    [SerializeField] private string alreadyHaveTxt  = "Kart zaten sende";

    public string InteractionText =>
        questManager.hasIstanbulCard ? alreadyHaveTxt : interactionTxt;

    public void Interact()
    {
        if (questManager.hasIstanbulCard) return;

        questManager.PickupIstanbulCard();
        SoundManager.PlaySound(SoundType.GunPickup);

        Destroy(gameObject); 
    }
}