// Assets/Scripts/Objects/WeaponPickupObject.cs
using UnityEngine;

public class WeaponPickupObject : MonoBehaviour, IInteractable
{
    [Header("Quest")]
    [SerializeField] private PlayerQuestManager questManager;

    [Header("Weapon to enable on player")]
    [SerializeField] private GameObject weaponObject; 

    [SerializeField] private string interactionTxt = "Pick Up Gun (E)";
    public string InteractionText => interactionTxt;

    public void Interact()
    {
        if (questManager.hasWeapon) return;

        questManager.PickupWeapon();

        if (weaponObject != null)
            weaponObject.SetActive(true);

        Destroy(gameObject); 
    }
}