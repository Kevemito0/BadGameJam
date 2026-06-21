// Assets/Scripts/Objects/IstanbulCardPickup.cs
using UnityEngine;

public class IstanbulCardPickup : MonoBehaviour, IInteractable
{
    [Header("Quest")]
    [SerializeField] private PlayerQuestManager questManager;

    [SerializeField] private string interactionTxt  = "Get Card [E]";
    [SerializeField] private string alreadyHaveTxt  = "u have it";

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